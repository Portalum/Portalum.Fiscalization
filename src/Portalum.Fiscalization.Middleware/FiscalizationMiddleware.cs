using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Portalum.Fiscalization.Middleware.Models;
using Portalum.Fiscalization.Middleware.Services;
using Portalum.Fiscalization.Models;

namespace Portalum.Fiscalization.Middleware
{
    public class FiscalizationMiddleware
    {
        private readonly ILogger<FiscalizationMiddleware> _logger;
        private readonly FiscalizationCountryCode _fiscalizationCountryCode;
        private readonly FiscalizationConfig _fiscalizationConfig;
        private readonly EfstaClient _efstaClient;
        private readonly List<Article> _articles = new List<Article>();
        private readonly ITaxGroupService _taxGroupService;

        public event Func<Task<ulong>> GetNextSequentialReceiptNumber;

        public FiscalizationMiddleware(
            FiscalizationCountryCode fiscalizationCountryCode,
            FiscalizationConfig fiscalizationConfig,
            EfstaClient efstaClient,
            ILogger<FiscalizationMiddleware>? logger = null)
        {
            this._fiscalizationCountryCode = fiscalizationCountryCode;
            this._fiscalizationConfig = fiscalizationConfig;
            this._logger = logger ?? new NullLogger<FiscalizationMiddleware>();

            this._taxGroupService = fiscalizationCountryCode switch
            {
                FiscalizationCountryCode.DE => new GermanyTaxGroupService(),
                FiscalizationCountryCode.AT => new AustriaTaxGroupService(),
                _ => throw new NotImplementedException("TaxGroupService is invalid"),
            };

            this._efstaClient = efstaClient;
        }

        public async Task<bool> StartAsync(CancellationToken cancellationToken = default)
        {
            if (this._fiscalizationCountryCode != FiscalizationCountryCode.DE)
            {
                return true;
            }

            var request = new TransactionStartRequest
            {
                Transaction = new TransactionStartData
                {
                    EfstaSimpleReceipt = new EfstaSimpleReceipt
                    {
                        StroreId = this._fiscalizationConfig.StroreId,
                        CashRegisterTerminalNumber = this._fiscalizationConfig.CashRegisterTerminalNumber
                    }
                }
            };

            var response = await this._efstaClient.TransactionStartAsync(
                request,
                this._fiscalizationConfig.VatIdentificationNumber,
                cancellationToken: cancellationToken);

            if (response == null)
            {
                return false;
            }

            //response.TransactionCompletion.FiscalData.TransactionId

            return true;
        }

        public bool AddArticle(Article article)
        {
            //TODO: is first article...?
            //StartAsync?

            this._articles.Add(article);

            return true;
        }

        public async Task<FinishResponse> FinishAsync(CancellationToken cancellationToken = default)
        {
            var sumTotal = this._articles.Select(item => item.Quantity * item.Amount).Sum();

            var elements = this._articles.Select(item =>
            {
                return new PositionElementArticle
                {
                    ItemNumber = item.EanCode,
                    ItemIdentity = $"{item.UniqueId}",
                    PositionAmount = FormattableString.Invariant($"{item.Quantity * item.Amount:0.00}"),
                    Quantity = $"{item.Quantity}",
                    Description = item.Description,
                    UnitPrice = FormattableString.Invariant($"{item.Amount:0.00}"),
                    TaxGroup = this._taxGroupService.GetTaxGroupCode(item.Tax)
                };
            });

            var elementsWithTaxGroup = this._articles.Select(item =>
            {
                return new
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.Amount,
                    Tax = item.Tax,
                    TaxGroup = this._taxGroupService.GetTaxGroupCode(item.Tax)
                };
            });

            var taxTempInfos = elementsWithTaxGroup.GroupBy(o => new { o.TaxGroup, o.Tax }).Select(o => new
            {
                TaxGroup = o.Key.TaxGroup,
                Tax = o.Key.Tax,
                GrossAmount = o.Select(x => x.Quantity * x.UnitPrice).Sum()
            });

            var taxElements = taxTempInfos.Select(o =>
            {
                var netAmount = Math.Round(o.GrossAmount / (1 + (o.Tax / 100)), 2);

                return new TaxElement
                {
                    TaxGroup = o.TaxGroup,
                    GrossAmount = FormattableString.Invariant($"{o.GrossAmount:0.00}"),
                    TaxAmount = FormattableString.Invariant($"{o.GrossAmount - netAmount:0.00}"),
                    TaxPercent = FormattableString.Invariant($"{o.Tax:0.00}"),
                    NetAmount = FormattableString.Invariant($"{netAmount:0.00}")
                };
            }).ToArray();

            var sequentialReceiptNumber = await this.GetNextSequentialReceiptNumber?.Invoke();

            var request = new TransactionFinishRequest
            {
                Transaction = new TransactionFinishData
                {
                    EfstaSimpleReceipt = new EfstaSimpleReceipt
                    {
                        StroreId = this._fiscalizationConfig.StroreId,
                        CashRegisterTerminalNumber = this._fiscalizationConfig.CashRegisterTerminalNumber,
                        OperatorId = this._fiscalizationConfig.OperatorId,
                        OperatorName = this._fiscalizationConfig.OperatorName,
                        ReceiptDate = DateTime.Now,
                        ProcessStartTimestamp = DateTime.Now.AddMinutes(-1),
                        SequentialReceiptNumber = $"{sequentialReceiptNumber}",
                        Total = FormattableString.Invariant($"{sumTotal:0.00}"),
                        PositionElements = elements.ToArray(),
                        PaymentElements = new PaymentElement[]
                        {
                            new PaymentElement
                            {
                                Description = "Cash",
                                PaymentAmount = FormattableString.Invariant($"{sumTotal:0.00}")
                            }
                        },
                        TaxElements = taxElements
                    }
                }
            };

            var response = await this._efstaClient.TransactionFinishAsync(
                request,
                this._fiscalizationConfig.VatIdentificationNumber,
                cancellationToken: cancellationToken);

            if (response == null)
            {
                return null;
            }

            return new FinishResponse
            {
                FiscalCode = response.TransactionCompletion.FiscalData.Code,
                Warnings = response.TransactionCompletion.Result.Warnings,
            };
        }
    }
}
