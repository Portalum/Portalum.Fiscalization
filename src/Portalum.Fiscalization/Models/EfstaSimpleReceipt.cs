using System;
using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class EfstaSimpleReceipt
    {
        /// <summary>
        /// Receipt Date
        /// </summary>
        /// <remarks>
        /// Seconds have to be specified according to fiscal law
        /// </remarks>
        [JsonPropertyName("D")]
        public DateTime ReceiptDate { get; set; }

        /// <summary>
        /// Date Time Start
        /// </summary>
        /// <remarks>
        /// Timestamp of the process start (The first item a customer orders in the restaurant for example a beer)
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("D0")]
        public DateTime ProcessStartTimestamp { get; set; }

        /// <summary>
        /// Date Time Start
        /// </summary>
        /// <remarks>
        /// Timestamp of the process start
        /// Required for Germany
        /// </remarks>
        public Customer Customer { get; set; }

        /// <summary>
        /// Operator Id
        /// </summary>
        /// <remarks>
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("Opr")]
        public string OperatorId { get; set; }

        /// <summary>
        /// Operator Name
        /// </summary>
        /// <remarks>
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("OprN")]
        public string OperatorName { get; set; }

        /// <summary>
        /// Nonfiscal Transaction Type
        /// </summary>
        /// <remarks>
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("NF")]
        public string NonfiscalTransactionType { get; set; }

        /// <summary>
        /// Void Transaction
        /// </summary>
        /// <remarks>
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("Void")]
        public string VoidTransaction { get; set; }

        /// <summary>
        /// StroreId
        /// </summary>
        /// <remarks>
        /// Transaction Location
        /// </remarks>
        [JsonPropertyName("TL")]
        public string StroreId { get; set; }

        /// <summary>
        /// Cash Register Terminal Number
        /// </summary>
        [JsonPropertyName("TT")]
        public string CashRegisterTerminalNumber { get; set; }

        /// <summary>
        /// Sequential receipt number
        /// </summary>
        /// <remarks>
        /// If only fiscally mandatory transactions are registered, continuous(gapless) sequence of
        /// receipt numbers has to be shown by TLOG of the cashier system
        /// Default value: TN is incremented automatically starting with "1".
        /// </remarks>
        [JsonPropertyName("TN")]
        public string SequentialReceiptNumber { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        /// <remarks>
        /// Amount of total sum of collected means of payment.
        /// </remarks>
        [JsonPropertyName("T")]
        public string Total { get; set; }

        /// <summary>
        /// Transaction Point
        /// </summary>
        /// <remarks>
        /// Point of transaction
        /// Required for Germany
        /// </remarks>
        [JsonPropertyName("TP")]
        public string TransactionPoint { get; set; }

        /// <summary>
        /// Array of position lines
        /// </summary>
        [JsonPropertyName("PosA")]
        public PositionElementBase[] PositionElements { get; set; }

        /// <summary>
        /// Array payment elements
        /// </summary>
        [JsonPropertyName("PayA")]
        public PaymentElement[] PaymentElements { get; set; }

        /// <summary>
        /// Array tax elements
        /// </summary>
        /// <remarks>
        /// Auto-generated if ESR.PosA is available
        /// </remarks>
        [JsonPropertyName("TaxA")]
        public TaxElement[] TaxElements { get; set; }

        /// <summary>
        /// Receipt Header
        /// </summary>
        [JsonPropertyName("Head")]
        public EfstaSimpleReceiptHeader[] Headers { get; set; }

        /// <summary>
        /// Receipt Footer
        /// </summary>
        [JsonPropertyName("Foot")]
        public EfstaSimpleReceiptFooter[] Footers { get; set; }
    }
}