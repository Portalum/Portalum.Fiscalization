namespace Portalum.Fiscalization.Middleware.Models
{
    public class Article
    {
        public string UniqueId { get; set; }
        public string EanCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
        public int Quantity { get; set; }
    }
}
