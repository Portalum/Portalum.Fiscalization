namespace Portalum.Fiscalization.SimplePos.Models
{
    public class ShoppingCartItem
    {
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }

        public decimal PriceTotal
        {
            get { return this.Quantity * this.PricePerUnit; }
        }
    }
}
