using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for ShoppingCartUserControl.xaml
    /// </summary>
    public partial class ShoppingCartUserControl : UserControl
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountingService _accountingService;

        public ShoppingCartUserControl()
        {
            var serviceProvider = ServiceContainer.Instance.ServiceProvider;
            var articleService = serviceProvider.GetService<IArticleRepository>();
            this._shoppingCartService = serviceProvider.GetService<IShoppingCartService>();
            this._accountingService = serviceProvider.GetService<IAccountingService>();

            this.RefreshCartItemCollection();

            this._shoppingCartService.CollectionChanged += this.RefreshCartItemCollection;

            this.InitializeComponent();
        }

        private void RefreshCartItemCollection()
        {
            var items = this._shoppingCartService.GetItemsAsync().GetAwaiter().GetResult();
            var itemCollection = new ObservableCollection<ShoppingCartItem>(items);
            this.DataContext = itemCollection;
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var items = await this._shoppingCartService.GetItemsAsync();
            if (!await this._accountingService.PrintReceiptAsync(items))
            {
                return;
            }

            await this._shoppingCartService.CompletePurchaseAsync();
            this.RefreshCartItemCollection();
        }
    }
}
