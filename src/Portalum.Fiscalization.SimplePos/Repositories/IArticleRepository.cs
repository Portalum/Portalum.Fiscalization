using Portalum.Fiscalization.SimplePos.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Repositories
{
    public interface IArticleRepository
    {
        Task<Article[]> QueryAsync(
            string filter,
            CancellationToken cancellationToken = default);
    }
}