using System;
using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class EfstaSimpleReceipt
    {
        /// <summary>
        /// The Transaction Id is returned from the start of a transaction and has to be transmitted in finish transaction request.
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TID</c>
        /// </remarks>
        [JsonPropertyName("TID")]
        public DateTime? TransactionId { get; set; }

        /// <summary>
        /// Receipt Date, seconds have to be specified according to fiscal law
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: D</c>
        /// </remarks>
        [JsonPropertyName("D")]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// Date Time Start, timestamp of the process start (The first item a customer orders in the restaurant for example a beer)
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: D0</c>
        /// </remarks>
        [JsonPropertyName("D0")]
        public DateTime? ProcessStartTimestamp { get; set; }

        /// <summary>
        /// Customer data if registered for invoice or if required by law
        /// <para>Required for Germany, France, Italy, Poland</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Ctm</c>
        /// </remarks>
        [JsonPropertyName("Ctm")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Operator Id
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Opr</c>
        /// </remarks>
        [JsonPropertyName("Opr")]
        public string OperatorId { get; set; }

        /// <summary>
        /// Operator Name
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: OprN</c>
        /// </remarks>
        [JsonPropertyName("OprN")]
        public string OperatorName { get; set; }

        /// <summary>
        /// Nonfiscal Transaction Type
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: NF</c>
        /// </remarks>
        [JsonPropertyName("NF")]
        public string NonfiscalTransactionType { get; set; }

        /// <summary>
        /// Void Transaction
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Void</c>
        /// </remarks>
        [JsonPropertyName("Void")]
        public string VoidTransaction { get; set; }

        /// <summary>
        /// StroreId, Transaction Location
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TL</c>
        /// </remarks>
        [JsonPropertyName("TL")]
        public string StroreId { get; set; }

        /// <summary>
        /// Cash Register Terminal Number
        /// </summary>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TT</c>
        /// </remarks>
        [JsonPropertyName("TT")]
        public string CashRegisterTerminalNumber { get; set; }

        /// <summary>
        /// Sequential receipt number, if only fiscally mandatory transactions are registered, continuous(gapless) sequence of
        /// receipt numbers has to be shown by TLOG of the cashier system
        /// Default value: TN is incremented automatically starting with "1".
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TN</c>
        /// </remarks>
        [JsonPropertyName("TN")]
        public string SequentialReceiptNumber { get; set; }

        /// <summary>
        /// Total, amount of total sum of collected means of payment.
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: T</c>
        /// </remarks>
        [JsonPropertyName("T")]
        public string Total { get; set; }

        /// <summary>
        /// Transaction Point, point of transaction
        /// <para>Required for Germany</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TP</c>
        /// </remarks>
        [JsonPropertyName("TP")]
        public string TransactionPoint { get; set; }

        /// <summary>
        /// Array of position lines
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: PosA</c>
        /// </remarks>
        [JsonPropertyName("PosA")]
        public PositionElementBase[] PositionElements { get; set; }

        /// <summary>
        /// Array payment elements
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: PayA</c>
        /// </remarks>
        [JsonPropertyName("PayA")]
        public PaymentElement[] PaymentElements { get; set; }

        /// <summary>
        /// Array tax elements
        /// <para>Auto-generated if ESR.PosA is available</para>
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: TaxA</c>
        /// </remarks>
        [JsonPropertyName("TaxA")]
        public TaxElement[] TaxElements { get; set; }

        /// <summary>
        /// Receipt Header
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Head</c>
        /// </remarks>
        [JsonPropertyName("Head")]
        public EfstaSimpleReceiptHeader[] Headers { get; set; }

        /// <summary>
        /// Receipt Footer
        /// </summary>
        /// <remarks>
        /// <c>Efsta EFR Field: Foot</c>
        /// </remarks>
        [JsonPropertyName("Foot")]
        public EfstaSimpleReceiptFooter[] Footers { get; set; }
    }
}