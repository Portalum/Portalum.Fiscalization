using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portalum.Fiscalization;
using Portalum.Fiscalization.Models;
using Portalum.Fiscalization.TestConsole;
using System.Diagnostics;

var services = new ServiceCollection();
services.AddLogging(configure =>
{
    configure.SetMinimumLevel(LogLevel.Trace);
    configure.AddConsole();
});
services.AddTransient<LoggingHandler>();
services.AddTransient<EfstaClient>();
services.AddHttpClient<EfstaClient>(configure =>
{
    configure.BaseAddress = new Uri("http://localhost:5618");
})
.AddHttpMessageHandler<LoggingHandler>();

var client = services.BuildServiceProvider().GetService<EfstaClient>();

var registerRequest = new RegisterRequest
{
    Transaction = new Transaction
    {
        EfstaSimpleReceipt = new EfstaSimpleReceipt
        {
            Headers = new EfstaSimpleReceiptHeader[]
            {
                new EfstaSimpleReceiptHeader
                {
                    Text = "Es bediente Sie Frau Gabriele"
                }
            },
            Footers = new EfstaSimpleReceiptFooter[]
            {
                new EfstaSimpleReceiptFooter
                {
                    Text = "Danke für Ihren Einkauf"
                }
            },
            ReceiptDate = DateTime.Today,
            StroreId = "001",
            CashRegisterTerminalNumber = "1",
            SequentialReceiptNumber = "2090",
            Total = "5.00",
            PositionElements = new PositionElementBase[]
            {
                new PositionElementArticle
                {
                    PositionNumber = "1",
                    ItemNumber = "1234455",
                    Description = "Hamburger",
                    TaxGroup = "B",
                    PositionAmount = "5.00",
                    Quantity = "1"
                },
                new PositionElementLine
                {
                    LineAmount = "----------------------"
                }
            },
            PaymentElements = new PaymentElement[]
            {
                new PaymentElement
                {
                    Description = "Cash",
                    PaymentAmount = "5.00",
                }
            },
            TaxElements = new TaxElement[]
            {
                new TaxElement
                {
                    TaxGroup = "B",
                    TaxPercent = "10",
                    NetAmount = "4.55",
                    TaxAmount = "0.45",
                    GrossAmount = "5.00"
                }
            }
        }
    }
};

var transactionStartRequest = new TransactionStartRequest
{
    Temp = new Temp
    {
        EfstaSimpleReceipt = new EfstaSimpleReceipt
        {
            StroreId = "1000",
            CashRegisterTerminalNumber = "1"
        }
    }
};

var response1 = await client.Register1Async(transactionStartRequest, "ATU57780814");
var x = response1.Temp.Fis.OperationStart;

var state = await client.GetStateAsync();
if (!state.Online)
{
    Console.WriteLine("Efsta is not online");
    return;
}

var lastNumber = 3600;
var stopwatch = new Stopwatch();

for (var i = 0; i < 100; i++)
{
    var sequentialReceiptNumber = lastNumber + i;
    registerRequest.Transaction.EfstaSimpleReceipt.SequentialReceiptNumber = $"{sequentialReceiptNumber}";

    stopwatch.Start();
    var response = await client.RegisterAsync(registerRequest, "ATU57780814");
    stopwatch.Stop();

    Console.WriteLine($"{response.TransactionCompletion.Result.ResultCode} / {stopwatch.Elapsed.TotalMilliseconds}ms");

    stopwatch.Reset();

   // await Task.Delay(1000);
}
