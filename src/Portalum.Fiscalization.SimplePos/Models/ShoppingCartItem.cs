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

        //public override string ToString()
        //{
        //    var totalPrice = this.Quantity * this.PricePerUnit;
        //    return $"{this.Quantity}x {this.ArticleName.PadRight(60 - this.ArticleName.Length, ' ')} {totalPrice:0.00}€";
        //}
    }
}
