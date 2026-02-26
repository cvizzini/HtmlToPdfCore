using System.Text;
using HtmlToPdfCore.Models;
using Stubble.Core;
using Stubble.Core.Builders;

namespace HtmlToPdfCore;

/// <summary>
/// Renders a Mustache HTML template against a strongly-typed view model
/// and writes the result to disk.
/// </summary>
public class HtmlParser
{
    private readonly StubbleVisitorRenderer _renderer =
        new StubbleBuilder().Build();

    /// <summary>
    /// Asynchronously renders <paramref name="htmlFilePath"/> with <paramref name="data"/>
    /// and writes the output to <paramref name="outputHtmlFileName"/>.
    /// </summary>
    /// <param name="htmlFilePath">Path to the Mustache HTML template file.</param>
    /// <param name="data">View model whose properties map to template placeholders.</param>
    /// <param name="outputHtmlFileName">Output HTML file path to convert to PDF.</param>
    public async Task ParseAsync(
        string htmlFilePath,
        object data,
        string outputHtmlFileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(htmlFilePath);
        ArgumentException.ThrowIfNullOrEmpty(outputHtmlFileName);
        ArgumentNullException.ThrowIfNull(data);

        string template = await File.ReadAllTextAsync(htmlFilePath);
        string rendered = await _renderer.RenderAsync(template, data);
        await File.WriteAllTextAsync(outputHtmlFileName, rendered);
    }

    public QuoteTemplateData BuildTemplateData() => new(
        CompanyName:   "TEST COMPANY (PTY) LTD",
        CompanyNumber: "CC 101",
        To:            "Test Client",
        Attention:     "John Doe",
        Date:          DateTime.Now.ToShortDateString(),
        QuoteNumber:   "12345",
        Items:         BuildFakeTable(),
        Conditions:    BuildConditionsOnNewLine(BuildFakeConditions()),
        User:          "Test Person",
        Watermark:     string.Empty); // e.g. "DRAFT"

    private static IEnumerable<QuoteItem> BuildFakeTable() =>
    [
        new("This is a test description 1", "1",             "R200.00"),
        new("This is a test description 2", "2",             "R400.00"),
        new("This is a test description 3", "3",             "R800.00"),
        new("",                             "Total Excl. VAT","R1400.00"),
    ];

    private static string BuildConditionsOnNewLine(string input) =>
        input.Replace(Environment.NewLine, " <br />");

    private static string BuildFakeConditions()
    {
        var sb = new StringBuilder();
        sb.AppendLine("- This quotation is valid for a period of 10 days from the date above");
        sb.AppendLine("- Goods supplied or repaired remain the property until paid for in full");
        sb.AppendLine("- These prices do not include shipping, customs clearance or VAT charges.");
        sb.AppendLine("- E. & O.E. (Errors and omissions excepted).");
        return sb.ToString();
    }
}
