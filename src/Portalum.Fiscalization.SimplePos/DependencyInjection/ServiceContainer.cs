using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.Docker;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
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
            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services, string countryCode)
        {
            services.AddLogging();
            services.AddSingleton<EfstaClient>();
            services.AddHttpClient<EfstaClient>(configure =>
            {
                configure.BaseAddress = new Uri("http://localhost:5618");
            });

            if (countryCode.Equals("at", StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<IArticleRepository, AustriaArticleRepository>();
            }
            else if (countryCode.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<IArticleRepository, GermanyArticleRepository>();
            }


            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<IAccountingService, AccountingService>();
        }
    }
}
