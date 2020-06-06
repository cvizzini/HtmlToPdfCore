using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace HtmlToPdfCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var htmlParser = new HtmlParser();
            var htmlTemplate = @$"{path}\Template\template.html";
            var htmlOutput = @$"{path}\Template\test123.html";
            var pdfOutput = "TestQuote.pdf";

            htmlParser.Parse(htmlTemplate, htmlParser.BuildDictionary(), htmlOutput);

            var htmlToPdfService = new HtmlToPdfService();
            htmlToPdfService.CreatePdf(htmlOutput, pdfOutput);

            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
        }
    }
}
