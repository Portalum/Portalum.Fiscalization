using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Helper;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.DependencyInjection
{
    public sealed class ServiceContainer
    {
        private static readonly Lazy<ServiceContainer> lazy = new(() => new ServiceContainer());
        public ServiceProvider ServiceProvider { get; private set; }

        public static ServiceContainer Instance { get { return lazy.Value; } }

        private ServiceContainer()
        {
            var countryCode = ConfigurationManager.AppSettings["CountryCode"];
            if (countryCode == null)
            {
                throw new NotImplementedException("CountryCode is required");
            }

            var serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection, countryCode);
            Task.Run(async () => await this.StartDockerAsync(countryCode));

            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private async Task StartDockerAsync(string countryCode)
        {
            await DockerHelper.CleanupAsync();

            if (countryCode.Equals("at", StringComparison.OrdinalIgnoreCase))
            {
                await DockerHelper.StartAsync("efstait/efsta_at");
            }
            else if (countryCode.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                await DockerHelper.StartAsync("efstait/efsta_de");
            }
        }

        private void ConfigureServices(IServiceCollection services, string countryCode)
        {
            if (countryCode.Equals("at", StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<IArticleRepository, AustriaArticleRepository>();
                services.AddSingleton<ITaxGroupService, AustriaTaxGroupService>();
            }
            else if (countryCode.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<IArticleRepository, GermanyArticleRepository>();
                services.AddSingleton<ITaxGroupService, GermanyTaxGroupService>();
            }

            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<IAccountingService, AccountingService>();
        }
    }
}
