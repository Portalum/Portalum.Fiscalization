namespace Portalum.Fiscalization.SimplePos.Models
{
    public class Article
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Tax { get; set; }
        public string EanCode { get; set; }
    }
}
