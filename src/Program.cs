using System.Reflection;
using HtmlToPdfCore;
using HtmlToPdfCore.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

    // #5 - Path constants extracted from inline magic strings
    const string TemplateName    = "template.html";
    const string HtmlOutputName  = "test123.html";
    const string PdfOutputName   = "TestQuote.pdf";

    var htmlTemplatePath = Path.Combine(basePath, "Template", TemplateName);
    var htmlOutputPath   = Path.Combine(basePath, "Template", HtmlOutputName);

    var htmlParser = new HtmlParser();

    // #6 - Sample data lives in Program.cs, not inside HtmlParser
    var templateData = SampleDataFactory.BuildQuoteTemplateData();

    await htmlParser.ParseAsync(htmlTemplatePath, templateData, htmlOutputPath);

    var htmlToPdfService = new HtmlToPdfService(Log.Logger);

    // #4 - CreatePdfAsync is now properly async
    await htmlToPdfService.CreatePdfAsync(htmlOutputPath, PdfOutputName);

    Log.Information("Press enter to quit...");
    Console.ReadLine();
}
finally
{
    await Log.CloseAndFlushAsync();
}
