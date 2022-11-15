using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Portalum.Fiscalization.Middleware;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public class AccountingService : IAccountingService, IDisposable
    {
        private readonly IArticleRepository _articleRepository;

        private readonly string _dataFile = "SequentialReceiptNumber.data";
        private ulong _lastSequentialReceiptNumber;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private readonly FiscalizationMiddleware _fiscalizationMiddleware;

        public AccountingService(
            IArticleRepository articleRepository,
            EfstaClient efstaClient)
        {
            this._articleRepository = articleRepository;

            this.LoadLastSequentialReceiptNumberAsync().GetAwaiter().GetResult();

            var fiscalizationConfig = this.LoadFiscalizationConfig();
            var fiscalizationCountryCode = FiscalizationCountryCode.Unknown;

            var countryCode = ConfigurationManager.AppSettings["CountryCode"];
            if (countryCode.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                fiscalizationCountryCode = FiscalizationCountryCode.DE;
            }
            else if (countryCode.Equals("at", StringComparison.OrdinalIgnoreCase))
            {
                fiscalizationCountryCode = FiscalizationCountryCode.AT;
            }

            this._fiscalizationMiddleware = new FiscalizationMiddleware(
                fiscalizationCountryCode,
                fiscalizationConfig,
                efstaClient
            );

            this._fiscalizationMiddleware.GetNextSequentialReceiptNumber += this.GetNextSequentialReceiptNumberAsync;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._fiscalizationMiddleware.GetNextSequentialReceiptNumber -= this.GetNextSequentialReceiptNumberAsync;
            }
        }

        private async Task<ulong> GetNextSequentialReceiptNumberAsync()
        {
            this._lastSequentialReceiptNumber++;
            await this.SaveLastSequentialReceiptNumberAsync();
            return this._lastSequentialReceiptNumber;
        }

        private FiscalizationConfig LoadFiscalizationConfig()
        {
            var storeId = ConfigurationManager.AppSettings["StroreId"];
            var cashRegisterTerminalNumber = ConfigurationManager.AppSettings["CashRegisterTerminalNumber"];
            var operatorId = ConfigurationManager.AppSettings["OperatorId"];
            var operatorName = ConfigurationManager.AppSettings["OperatorName"];
            var vatIdentificationNumber = ConfigurationManager.AppSettings["VatIdentificationNumber"];

            return new FiscalizationConfig
            {
                StroreId = storeId,
                CashRegisterTerminalNumber = cashRegisterTerminalNumber,
                OperatorId = operatorId,
                OperatorName = operatorName,
                VatIdentificationNumber = vatIdentificationNumber
            };
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
            var articles = await this._articleRepository.QueryAsync("");

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var article = articles.Where(article => article.Id == shoppingCartItem.ArticleId).FirstOrDefault();

                this._fiscalizationMiddleware.AddArticle(new Middleware.Models.Article
                {
                     UniqueId = $"{shoppingCartItem.ArticleId}",
                     Amount = shoppingCartItem.PricePerUnit,
                     Quantity = shoppingCartItem.Quantity,
                     Description = shoppingCartItem.ArticleName,
                     EanCode = article.EanCode,
                     Tax = article.Tax
                });
            }

            if (await this._syncLock.WaitAsync(10000))
            {
                try
                {
                    var response = await this._fiscalizationMiddleware.FinishAsync();

                    var receiptPrinterIpAddress = ConfigurationManager.AppSettings["ReceiptPrinterIpAddress"];
                    var storeId = ConfigurationManager.AppSettings["StroreId"];
                    var cashRegisterTerminalNumber = ConfigurationManager.AppSettings["CashRegisterTerminalNumber"];
                    var operatorName = ConfigurationManager.AppSettings["OperatorName"];

                    var printJobData = new PrintJobData
                    {
                        ShoppingCartItems = shoppingCartItems,
                        ReceiptPrinterIpAddress = receiptPrinterIpAddress,
                        FiscalData = response.FiscalCode,
                        AdditionalLines = response.Warnings,
                        Cashier = operatorName,
                        PosUniqueIdentifier = $"{storeId}-{cashRegisterTerminalNumber}",
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
