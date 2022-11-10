using Portalum.Fiscalization.SimplePos.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        public readonly Article[] _articles;

        public ArticleRepository()
        {
            this._articles = new Article[]
            {
                new Article
                {
                    Id = 1,
                    Name = "Cola 0,3",
                    EanCode = "5449000050205",
                    GrossPrice = 2.5m,
                    Tax = 20
                },
                new Article
                {
                    Id = 2,
                    Name = "Eier Mie Nudeln",
                    EanCode = "4023900540564",
                    GrossPrice = 5.0m,
                    Tax = 10
                },
                new Article
                {
                    Id = 3,
                    Name = "Shampoo For Men",
                    EanCode = "4015100292312",
                    GrossPrice = 10.0m,
                    Tax = 20
                }
            };
        }

        public Task<Article[]> QueryAsync(
            string filter,
            CancellationToken cancellationToken = default)
        {
            Func<Article, bool> query = (article) => article.Name.Contains(filter, StringComparison.OrdinalIgnoreCase);

            var filteredArticles = this._articles.Where(query).ToArray();
            return Task.FromResult(filteredArticles);
        }
    }
}
