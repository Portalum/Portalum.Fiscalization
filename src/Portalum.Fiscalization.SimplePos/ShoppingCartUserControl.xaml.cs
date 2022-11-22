using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for ShoppingCartUserControl.xaml
    /// </summary>
    public partial class ShoppingCartUserControl : UserControl
    {
        private IShoppingCartService _shoppingCartService;
        private readonly IAccountingService _accountingService;

        public ShoppingCartUserControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
                return;
            }

            var serviceProvider = ServiceContainer.Instance.ServiceProvider;
            var articleService = serviceProvider.GetService<IArticleRepository>();
            this._accountingService = serviceProvider.GetService<IAccountingService>();

            this.InitializeComponent();
        }

        public void SetShoppingCartService(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
            this._shoppingCartService.CollectionChanged += this.RefreshCartItemCollection;

            this.RefreshCartItemCollection();
        }

        private void RefreshCartItemCollection()
        {
            var items = this._shoppingCartService.GetItemsAsync().GetAwaiter().GetResult();
            var itemCollection = new ObservableCollection<ShoppingCartItem>(items);
            this.DataContext = itemCollection;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var items = await this._shoppingCartService.GetItemsAsync();
            if (!await this._accountingService.PrintReceiptAsync(items))
            {
                MessageBox.Show("Cannot print receipt, check printer config and efsta config", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await this._shoppingCartService.CompletePurchaseAsync();
            this.RefreshCartItemCollection();
        }

        private async void DockPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dockPanel = sender as DockPanel;
            if (dockPanel.Tag is ShoppingCartItem shoppingCartItem)
            {
                await this._shoppingCartService.RemoveArticleAsync(shoppingCartItem.ArticleId);
            }
        }
    }
}
