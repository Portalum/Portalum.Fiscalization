using Microsoft.Extensions.DependencyInjection;
using Portalum.Fiscalization.SimplePos.DependencyInjection;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using Portalum.Fiscalization.SimplePos.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for ArticleSelectorUserControl.xaml
    /// </summary>
    public partial class ArticleSelectorUserControl : UserControl
    {
        private IShoppingCartService _shoppingCartService;

        public ArticleSelectorUserControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
                return;
            }

            var serviceProvider = ServiceContainer.Instance.ServiceProvider;
            var articleRepository = serviceProvider.GetService<IArticleRepository>();

            var articles = articleRepository.QueryAsync("").GetAwaiter().GetResult();
            var articleCollection = new ObservableCollection<Article>(articles);
            this.DataContext = articleCollection;

            this.InitializeComponent();
        }

        public void SetShoppingCartService(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

        private async void ArticleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var article = button.DataContext as Article;
            await this._shoppingCartService.AddArticleAsync(article);
        }
    }
}
