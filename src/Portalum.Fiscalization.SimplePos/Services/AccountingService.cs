using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Microsoft.Extensions.Logging.Abstractions;
using Portalum.Fiscalization.Models;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITaxGroupService _taxGroupService;

        private readonly string _dataFile = "SequentialReceiptNumber.data";
        private ulong _lastSequentialReceiptNumber;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        public AccountingService(
            IArticleRepository articleRepository,
            ITaxGroupService taxGroupService)
        {
            this._articleRepository = articleRepository;
            this._taxGroupService = taxGroupService;

            this.LoadLastSequentialReceiptNumberAsync().GetAwaiter().GetResult();

            this._httpClient = new HttpClient();
            this._httpClient.BaseAddress = new Uri("http://localhost:5618");
        }

        private async Task LoadLastSequentialReceiptNumberAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (!File.Exists(this._dataFile))
                {
                    this._lastSequentialReceiptNumber = 1;
                    return;
                }

                var temp = await File.ReadAllTextAsync(this._dataFile, cancellationToken).ConfigureAwait(false);
                if (!ulong.TryParse(temp, out var sequentialReceiptNumber))
                {
                    this._lastSequentialReceiptNumber = 0;
                    return;
                }

                this._lastSequentialReceiptNumber = sequentialReceiptNumber;

                
            }
            catch (Exception exception)
            {
                this._lastSequentialReceiptNumber = 0;
            }
        }

        private async Task SaveLastSequentialReceiptNumberAsync(CancellationToken cancellationToken = default)
        {
            await File.WriteAllTextAsync(this._dataFile, $"{this._lastSequentialReceiptNumber}", cancellationToken);
        }

        public async Task<bool> PrintReceiptAsync(ShoppingCartItem[] shoppingCartItems)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            var sumTotal = shoppingCartItems.Select(item => item.Quantity * item.PricePerUnit).Sum();

            var articles = await this._articleRepository.QueryAsync("");

            var storeId = ConfigurationManager.AppSettings["StroreId"];
            var cashRegisterTerminalNumber = ConfigurationManager.AppSettings["CashRegisterTerminalNumber"];
            var operatorId = ConfigurationManager.AppSettings["OperatorId"];
            var operatorName = ConfigurationManager.AppSettings["OperatorName"];
            var vatIdentificationNumber = ConfigurationManager.AppSettings["VatIdentificationNumber"];
            var receiptPrinterIpAddress = ConfigurationManager.AppSettings["ReceiptPrinterIpAddress"];

            var elements = shoppingCartItems.Select(item =>
            {
                var article = articles.Where(article => article.Id == item.ArticleId).FirstOrDefault();

                return new PositionElementArticle
                {
                    ItemNumber = article.EanCode,
                    ItemIdentity = $"{article.Id}",
                    PositionAmount = $"{item.Quantity * item.PricePerUnit:0.00}",
                    Quantity = $"{item.Quantity}",
                    Description = article.Name,
                    //UnitPrice = $"{item.PricePerUnit:0.00}",
                    TaxGroup = this._taxGroupService.GetTaxGroupCode(article.Tax)
                };
            });

            var elementsWithTaxGroup = shoppingCartItems.Select(item =>
            {
                var article = articles.Where(article => article.Id == item.ArticleId).FirstOrDefault();

                return new
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.PricePerUnit,
                    Tax = article.Tax,
                    TaxGroup = this._taxGroupService.GetTaxGroupCode(article.Tax)
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
                    GrossAmount = $"{o.GrossAmount:0.00}",
                    TaxAmount = $"{o.GrossAmount - netAmount:0.00}",
                    TaxPercent = $"{o.Tax:0.00}",
                    NetAmount = $"{netAmount:0.00}"
                };
            }).ToArray();

            var finishRequest = new TransactionFinishRequest
            {
                Transaction = new TransactionFinishData
                {
                    EfstaSimpleReceipt = new EfstaSimpleReceipt
                    {
                        StroreId = storeId,
                        CashRegisterTerminalNumber = cashRegisterTerminalNumber,
                        OperatorId = operatorId,
                        OperatorName = operatorName,
                        ReceiptDate = DateTime.Now,
                        ProcessStartTimestamp = DateTime.Now.AddMinutes(-1),
                        SequentialReceiptNumber = $"{this._lastSequentialReceiptNumber}",
                        Total = $"{sumTotal:0.00}",
                        PositionElements = elements.ToArray(),
                        PaymentElements = new PaymentElement[]
                        {
                            new PaymentElement
                            {
                                Description = "Cash",
                                PaymentAmount = $"{sumTotal:0.00}"
                            }
                        },
                        TaxElements = taxElements
                    }
                }
            };

            if (await this._syncLock.WaitAsync(10000))
            {
                try
                {
                    this._lastSequentialReceiptNumber++;

                    var efstaClient = new EfstaClient(new NullLogger<EfstaClient>(), this._httpClient);
                    var response = await efstaClient.TransactionFinishAsync(finishRequest, vatIdentificationNumber);
                    if (response == null)
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(response.TransactionCompletion.FiscalData?.Code))
                    {
                        return false;
                    }

                    

                    await this.SaveLastSequentialReceiptNumberAsync();

                    var printJobData = new PrintJobData
                    {
                        AdditionalLines = response.TransactionCompletion.Result.Warnings,
                        ShoppingCartItems = shoppingCartItems,
                        Cashier = operatorName,
                        PosUniqueIdentifier = $"{storeId}-{cashRegisterTerminalNumber}",
                        ReceiptPrinterIpAddress = receiptPrinterIpAddress,
                        FiscalData = response.TransactionCompletion.FiscalData.Code
                    };

                    return await this.PrintReceiptAsync(printJobData);
                }
                finally
                {
                    this._syncLock.Release();
                }
            }

            return false;
        }

        private async Task<bool> PrintReceiptAsync(PrintJobData printJobData)
        {
            var port = 9100;
            var printerSettings = new ImmediateNetworkPrinterSettings
            {
                ConnectionString = $"{printJobData.ReceiptPrinterIpAddress}:{port}",
                PrinterName = "PosReceiptPrinter001"
            };

            var printer = new ImmediateNetworkPrinter(printerSettings);

            try
            {
                var e = new EPSON();
                await printer.WriteAsync(
                  ByteSplicer.Combine(
                    e.SetStyles(PrintStyle.None),
                    e.SetStyles(PrintStyle.Bold),
                    e.CenterAlign(),
                    e.PrintLine(""),
                    e.PrintLine("PORTALUM"),
                    e.PrintLine("Riedgasse 50"),
                    e.PrintLine("6850 Dornbirn"),
                    e.PrintLine(""),
                    e.PrintLine($"Belegnummer: {DateTime.Today.Year}-{printJobData.PosUniqueIdentifier}-{this._lastSequentialReceiptNumber:000000}"),
                    e.PrintLine(""),
                    e.SetStyles(PrintStyle.Underline),
                    e.PrintLine($"Es bediente sie {printJobData.Cashier}"),
                    e.SetStyles(PrintStyle.None),
                    e.PrintLine("")
                  )
                );

                foreach (var shoppingCartItem in printJobData.ShoppingCartItems)
                {
                    var price = shoppingCartItem.PricePerUnit * shoppingCartItem.Quantity;

                    await printer.WriteAsync(
                      ByteSplicer.Combine(
                        e.LeftAlign(),
                        e.SetStyles(PrintStyle.DoubleHeight),
                        e.PrintLine($"{shoppingCartItem.Quantity}x {shoppingCartItem.ArticleName,-30}{price:0.00} EUR")
                      )
                    );
                }

                var total = printJobData.ShoppingCartItems.Select(o => o.PriceTotal).Sum();

                await printer.WriteAsync(
                      ByteSplicer.Combine(
                        e.RightAlign(),
                        e.PrintLine("------------------------------------------"),
                        e.SetStyles(PrintStyle.DoubleHeight),
                        e.PrintLine($"TOTAL: {total:0.00} EUR")
                      )
                    );

                if (printJobData.AdditionalLines != null)
                {
                    foreach (var additionalLine in printJobData.AdditionalLines)
                    {
                        await printer.WriteAsync(
                          ByteSplicer.Combine(
                            e.SetStyles(PrintStyle.None),
                            e.RightAlign(),
                            e.PrintLine(additionalLine)
                          )
                        );
                    }
                }

                await printer.WriteAsync(
                  ByteSplicer.Combine(
                    e.PrintLine(""),
                    e.LeftAlign(),
                    e.SetStyles(PrintStyle.FontB),
                    e.PrintLine(""),
                    e.PrintLine(""),
                    e.CenterAlign(),
                    e.PrintQRCode(printJobData.FiscalData, TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_7),
                    e.PrintLine(""),
                    e.FullCutAfterFeed(10)
                  )
                );

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
