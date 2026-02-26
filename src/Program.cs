using System.Reflection;
using HtmlToPdfCore.Data;
using HtmlToPdfCore.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

    const string TemplateName   = "quote.html";
    const string HtmlOutputName = "quote-rendered.html";
    const string PdfOutputName  = "TestQuote.pdf";

    var templatePath   = Path.Combine(basePath, "Templates", TemplateName);
    var htmlOutputPath = Path.Combine(basePath, "Templates", HtmlOutputName);

    var templateData = SampleDataFactory.BuildQuoteTemplateData();

    var htmlParser = new HtmlParser();
    await htmlParser.ParseAsync(templatePath, templateData, htmlOutputPath);

    var pdfService = new HtmlToPdfService(Log.Logger);
    var pdfPath = await pdfService.CreatePdfAsync(htmlOutputPath, PdfOutputName);

    Log.Information("PDF saved to: {PdfPath}", pdfPath);
    Log.Information("Press enter to quit...");
    Console.ReadLine();
}
finally
{
    await Log.CloseAndFlushAsync();
}
