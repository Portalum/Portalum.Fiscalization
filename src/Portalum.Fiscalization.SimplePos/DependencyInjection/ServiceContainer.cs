using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System;

namespace Portalum.Fiscalization.SimplePos.DependencyInjection
{
    public sealed class ServiceContainer
    {
        private static readonly Lazy<ServiceContainer> lazy = new(() => new ServiceContainer());
        public ServiceProvider ServiceProvider { get; private set; }

        public static ServiceContainer Instance { get { return lazy.Value; } }

        private ServiceContainer()
        {
            var serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection);

            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IArticleRepository, ArticleRepository>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            services.AddSingleton<IAccountingService, AccountingService>();
        }
    }
}
