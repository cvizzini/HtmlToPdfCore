using System.Text;
using HtmlToPdfCore.Models;
using Stubble.Core;
using Stubble.Core.Builders;

namespace HtmlToPdfCore.Services;

/// <summary>
/// Renders a Mustache HTML template against a strongly-typed view model
/// and writes the rendered result to disk, ready for PDF conversion.
/// </summary>
public class HtmlParser
{
    private readonly StubbleVisitorRenderer _renderer = new StubbleBuilder().Build();

    /// <summary>
    /// Renders the Mustache template at <paramref name="templatePath"/> with the supplied
    /// <paramref name="data"/> and writes the output HTML to <paramref name="outputPath"/>.
    /// </summary>
    public async Task ParseAsync(string templatePath, object data, string outputPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(templatePath);
        ArgumentException.ThrowIfNullOrEmpty(outputPath);
        ArgumentNullException.ThrowIfNull(data);

        string template = await File.ReadAllTextAsync(templatePath);
        string rendered = await _renderer.RenderAsync(template, data);
        await File.WriteAllTextAsync(outputPath, rendered);
    }

    /// <summary>
    /// Replaces newline characters with HTML line break tags.
    /// Useful for rendering multi-line plain-text strings inside HTML templates.
    /// </summary>
    public static string ConvertNewLinesToHtmlBreaks(string input) =>
        input.Replace(Environment.NewLine, " <br />");
}
