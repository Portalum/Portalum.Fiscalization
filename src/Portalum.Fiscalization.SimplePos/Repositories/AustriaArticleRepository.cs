using Portalum.Fiscalization.SimplePos.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Repositories
{
    public class AustriaArticleRepository : IArticleRepository
    {
        public readonly Article[] _articles;

        public AustriaArticleRepository()
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
                    Name = "Wiener Schnitzel",
                    GrossPrice = 15.0m,
                    Tax = 10
                },
                new Article
                {
                    Id = 3,
                    Name = "Shampoo For Men",
                    EanCode = "4015100292312",
                    GrossPrice = 10.0m,
                    Tax = 20
                },
                new Article
                {
                    Id = 4,
                    Name = "1000 Blatt Kopierpapier",
                    EanCode = "4251529808516",
                    GrossPrice = 20.0m,
                    Tax = 20
                },
                new Article
                {
                    Id = 5,
                    Name = "BIC Kugelschreiber, blau",
                    EanCode = "0070330172883",
                    GrossPrice = 1.99m,
                    Tax = 20
                },
                new Article
                {
                    Id = 6,
                    Name = "Heineken Bier EW 6x0",
                    EanCode = "8712000900038",
                    GrossPrice = 8.40m,
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
