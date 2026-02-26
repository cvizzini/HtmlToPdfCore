using System.Text;
using HtmlToPdfCore.Models;

namespace HtmlToPdfCore;

/// <summary>
/// Builds hardcoded sample data for demonstrating PDF generation.
/// Kept separate from HtmlParser so the parser stays focused on rendering logic.
/// </summary>
public static class SampleDataFactory
{
    public static QuoteTemplateData BuildQuoteTemplateData() => new(
        CompanyName:   "TEST COMPANY (PTY) LTD",
        CompanyNumber: "CC 101",
        To:            "Test Client",
        Attention:     "John Doe",
        Date:          DateTime.Now.ToShortDateString(),
        QuoteNumber:   "12345",
        Items:         BuildLineItems(),
        Conditions:    HtmlParser.ConvertNewLinesToHtmlBreaks(BuildConditions()),
        User:          "Test Person",
        Watermark:     string.Empty);   // e.g. pass "DRAFT" to watermark the document

    private static IEnumerable<QuoteItem> BuildLineItems() =>
    [
        new("This is a test description 1", "1",              "R200.00"),
        new("This is a test description 2", "2",              "R400.00"),
        new("This is a test description 3", "3",              "R800.00"),
        new("",                             "Total Excl. VAT", "R1400.00"),
    ];

    private static string BuildConditions()
    {
        var sb = new StringBuilder();
        sb.AppendLine("- This quotation is valid for a period of 10 days from the date above");
        sb.AppendLine("- Goods supplied or repaired remain the property until paid for in full");
        sb.AppendLine("- These prices do not include shipping, customs clearance or VAT charges.");
        sb.AppendLine("- E. & O.E. (Errors and omissions excepted).");
        return sb.ToString();
    }
}
