using Portalum.Fiscalization.SimplePos.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public interface IShoppingCartService
    {
        event Action CollectionChanged;

        Task<ShoppingCartItem[]> GetItemsAsync();

        Task AddArticleAsync(
            Article article,
            CancellationToken cancellationToken = default);

        Task RemoveArticleAsync(
            int articleId,
            CancellationToken cancellationToken = default);

        Task CompletePurchaseAsync(CancellationToken cancellationToken = default);
    }
}