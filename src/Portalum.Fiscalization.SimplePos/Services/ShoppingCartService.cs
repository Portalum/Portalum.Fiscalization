using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ConcurrentDictionary<int, ArticleCartDetail> _items = new ConcurrentDictionary<int, ArticleCartDetail>();

        public event Action CollectionChanged;

        public ShoppingCartService(IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
        }

        public async Task<ShoppingCartItem[]> GetItemsAsync()
        {
            var articles = await this._articleRepository.QueryAsync("");

            var items = this._items.Select(item => {
                var article = articles.Where(o => o.Id == item.Key).FirstOrDefault();

                return new ShoppingCartItem
                {
                    ArticleId = item.Key,
                    ArticleName = article.Name,
                    Quantity = item.Value.Quantity,
                    PricePerUnit = article.GrossPrice
                };
            }).ToArray();

            return items;
        }

        public Task AddArticleAsync(
            Article article,
            CancellationToken cancellationToken = default)
        {
            if (this._items.TryGetValue(article.Id, out var articleCartDetail))
            {
                articleCartDetail.Quantity++;

                this.CollectionChanged?.Invoke();
                return Task.CompletedTask;
            }

            this._items.TryAdd(article.Id, new ArticleCartDetail
            {
                Quantity = 1
            });

            this.CollectionChanged?.Invoke();

            return Task.CompletedTask;
        }

        public Task RemoveArticleAsync(
            int articleId,
            CancellationToken cancellationToken = default)
        {
            if (!this._items.TryGetValue(articleId, out var articleCartDetail))
            {
                return Task.CompletedTask;
            }

            articleCartDetail.Quantity--;

            if (articleCartDetail.Quantity <= 0)
            {
                this._items.TryRemove(articleId, out _);
            }

            this.CollectionChanged?.Invoke();

            return Task.CompletedTask;
        }

        public Task CompletePurchaseAsync(CancellationToken cancellationToken = default)
        {
            this._items.Clear();

            return Task.CompletedTask;
        }
    }
}
