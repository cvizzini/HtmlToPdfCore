using System.Reflection;
using HtmlToPdfCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

    var htmlParser = new HtmlParser();
    var htmlTemplate = Path.Combine(path, "Template", "template.html");
    var htmlOutput = Path.Combine(path, "Template", "test123.html");
    var pdfOutput = "TestQuote.pdf";

    await htmlParser.ParseAsync(htmlTemplate, htmlParser.BuildTemplateData(), htmlOutput);

    var htmlToPdfService = new HtmlToPdfService(Log.Logger);
    htmlToPdfService.CreatePdf(htmlOutput, pdfOutput);

    Log.Information("Press enter to quit...");
    Console.ReadLine();
}
finally
{
    await Log.CloseAndFlushAsync();
}
