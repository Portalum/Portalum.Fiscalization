using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Services;
using System.Windows.Controls;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for PosPage.xaml
    /// </summary>
    public partial class PosPage : Page
    {
        public PosPage()
        {
            this.InitializeComponent();

            var serviceProvider = ServiceContainer.Instance.ServiceProvider;
            var shoppingCartService = serviceProvider.GetService<IShoppingCartService>();

            this.ArticleSelector.SetShoppingCartService(shoppingCartService);
            this.ShoppingCart.SetShoppingCartService(shoppingCartService);
        }
    }
}
