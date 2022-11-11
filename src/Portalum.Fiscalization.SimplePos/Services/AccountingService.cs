using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Microsoft.Extensions.Logging.Abstractions;
using Portalum.Fiscalization.Models;
using Portalum.Fiscalization.SimplePos.Models;
using Portalum.Fiscalization.SimplePos.Repositories;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly IArticleRepository _articleRepository;

        public AccountingService(IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
        }

        public async Task<bool> PrintReceiptAsync(ShoppingCartItem[] shoppingCartItems)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            var sumTotal = shoppingCartItems.Select(item => item.Quantity * item.PricePerUnit).Sum();

            var articles = await this._articleRepository.QueryAsync("");


            var elements = shoppingCartItems.Select(item =>
            {
                var article = articles.Where(article => article.Id == item.ArticleId).FirstOrDefault();

                return new PositionElementArticle
                {
                    ItemNumber = article.EanCode,
                    ItemIdentity = $"{article.Id}",
                    PositionAmount = $"{item.Quantity * item.PricePerUnit:0.00}",
                    Quantity = $"{item.Quantity:0.00}",
                    UnitPrice = $"{item.PricePerUnit:0.00}",
                    TaxGroup = "A"
                };
            });

            var registerRequest = new RegisterRequest
            {
                Transaction = new Transaction
                {
                    EfstaSimpleReceipt = new EfstaSimpleReceipt
                    {
                        StroreId = "ST1",
                        CashRegisterTerminalNumber = "1",
                        OperatorId = "1",
                        OperatorName = "Max Mustermann",
                        ReceiptDate = DateTime.Now,
                        ProcessStartTimestamp = DateTime.Now.AddMinutes(-1),
                        SequentialReceiptNumber = "1",
                        Total = $"{sumTotal:0.00}",
                        PositionElements = elements.ToArray(),
                        TaxElements = new TaxElement[]
                        {
                            new TaxElement
                            {
                                TaxGroup = "A",
                                GrossAmount = "12",
                                Net = "10",
                                TaxAmount = "1.2"
                            }
                        }
                    }
                }
            };

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5618");

            var efstaClient = new EfstaClient(new NullLogger<EfstaClient>(), httpClient);
            var response = await efstaClient.RegisterAsync(registerRequest, "ATU57780814");
            if (response == null)
            {
                return false;
            }

            //Console.WriteLine(response.TransactionCompletion.Result.ResultCode);


            var hostnameOrIp = "10.15.0.165";
            var port = 9100;
            var printer = new ImmediateNetworkPrinter(
                new ImmediateNetworkPrinterSettings
                {
                    ConnectionString = $"{hostnameOrIp}:{port}",
                    PrinterName = "TestPrinter"
                });

            try
            {
                var e = new EPSON();
                await printer.WriteAsync(
                  ByteSplicer.Combine(
                    e.CenterAlign(),
                    e.PrintLine(""),
                    e.PrintLine("PORTALUM"),
                    e.PrintLine("Riedgasse 50"),
                    e.PrintLine("6850 Dornbirn"),
                    e.PrintLine(""),
                    e.PrintLine("BelegNummer: 12345"),
                    e.PrintLine(""),
                    e.SetStyles(PrintStyle.Underline),
                    e.PrintLine("Es bediente sie Reinhard Feuerstein"),
                    e.SetStyles(PrintStyle.None),
                    e.PrintLine("")
                  )
                );

                foreach (var shoppingCartItem in shoppingCartItems)
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

                await printer.WriteAsync(
                  ByteSplicer.Combine(
                    e.PrintLine(""),
                    e.LeftAlign(),
                    e.SetStyles(PrintStyle.FontB),
                    e.PrintLine(""),
                    e.PrintLine(""),
                    e.CenterAlign(),
                    e.PrintQRCode(response.TransactionCompletion.FiscalData.Code, TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_7),
                    e.PrintLine(""),
                    e.FullCutAfterFeed(10)

                  )
                );
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }
    }
}
