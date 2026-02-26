namespace HtmlToPdfCore.Models;

/// <summary>Strongly-typed view model bound to the HTML Mustache template.</summary>
public record QuoteTemplateData(
    string CompanyName,
    string CompanyNumber,
    string To,
    string Attention,
    string Date,
    string QuoteNumber,
    IEnumerable<QuoteItem> Items,   // rendered by {{#Items}}...{{/Items}} in the template
    string Conditions,
    string User,
    string Watermark);