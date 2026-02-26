using System.Text;
using HtmlToPdfCore.Models;
using Stubble.Core;
using Stubble.Core.Builders;

namespace HtmlToPdfCore;

/// <summary>
/// Renders a Mustache HTML template against a strongly-typed view model
/// and writes the rendered result to disk, ready for PDF conversion.
/// </summary>
public class HtmlParser
{
    private readonly StubbleVisitorRenderer _renderer = new StubbleBuilder().Build();

    /// <summary>
    /// Renders the Mustache template at <paramref name="htmlFilePath"/> with the supplied
    /// <paramref name="data"/> and writes the output HTML to <paramref name="outputHtmlFilePath"/>.
    /// </summary>
    /// <param name="htmlFilePath">Path to the Mustache HTML template file.</param>
    /// <param name="data">View model whose properties map to template placeholders.</param>
    /// <param name="outputHtmlFilePath">Destination path for the rendered HTML output.</param>
    public async Task ParseAsync(
        string htmlFilePath,
        object data,
        string outputHtmlFilePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(htmlFilePath);
        ArgumentException.ThrowIfNullOrEmpty(outputHtmlFilePath);
        ArgumentNullException.ThrowIfNull(data);

        string template = await File.ReadAllTextAsync(htmlFilePath);
        string rendered = await _renderer.RenderAsync(template, data);
        await File.WriteAllTextAsync(outputHtmlFilePath, rendered);
    }

    // #7 - Renamed: ConvertNewLinesToHtmlBreaks is clearer about what it actually does
    // Also extracted as a standalone static helper (extension method candidate)
    public static string ConvertNewLinesToHtmlBreaks(string input) =>
        input.Replace(Environment.NewLine, " <br />");
}
