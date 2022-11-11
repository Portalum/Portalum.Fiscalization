namespace Portalum.Fiscalization.SimplePos.Models
{
    public class PrintJobData
    {
        public string ReceiptPrinterIpAddress { get; set; }
        public string Cashier { get; set; }
        public ShoppingCartItem[] ShoppingCartItems { get; set; }
        public string FiscalData { get; set; }
        public string PosUniqueIdentifier { get; set; }

        public string[] AdditionalLines { get; set; }
    }
}
