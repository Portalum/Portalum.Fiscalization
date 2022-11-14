using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Helper;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
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

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Sign_require", "TSE_Sim"),
                    //new KeyValuePair<string, string>("Offline", ""),
                    //new KeyValuePair<string, string>("Badge", ""),
                    //new KeyValuePair<string, string>("Proxy", ""),
                    //new KeyValuePair<string, string>("TaxId", ""),
                    //new KeyValuePair<string, string>("Sign_Cfg", ""),
                    //new KeyValuePair<string, string>("RN_TT", ""),
                    //new KeyValuePair<string, string>("Password", ""),
                    //new KeyValuePair<string, string>("Update_disable", ""),
                    //new KeyValuePair<string, string>("HttpServer_Port", "5620"),
                    //new KeyValuePair<string, string>("RootPath", ""),
                    //new KeyValuePair<string, string>("DiskQuota", "1000"),
                    //new KeyValuePair<string, string>("Attributes", ""),
                    //new KeyValuePair<string, string>("Notes", ""),
                    //new KeyValuePair<string, string>("@save", "save")
                });

                await Task.Delay(1000);

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://localhost:5618/profile?RN=def", formContent);
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
