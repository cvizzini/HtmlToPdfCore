using DinkToPdf;
using DinkToPdf.Contracts;
using Serilog;
using System.Reflection;

namespace HtmlToPdfCore.Services;

/// <summary>
/// Converts a rendered HTML file to PDF using DinkToPdf (libwkhtmltox wrapper).
/// Conversion happens in-process via the native library — no external process is spawned.
/// See: https://github.com/rdvojmoc/DinkToPdf
/// </summary>
public class HtmlToPdfService
{
    private readonly string _basePath;
    private readonly string _outputDirectory;
    private readonly ILogger _logger;
    private readonly IConverter _converter;

    public HtmlToPdfService(ILogger logger)
    {
        _logger = logger.ForContext<HtmlToPdfService>();

        _basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
            ?? throw new InvalidOperationException("Could not resolve the entry assembly location.");

        _outputDirectory = Path.Combine(_basePath, "Output");
        CreateOutputDirectory();

        // SynchronizedConverter is the thread-safe wrapper recommended for shared use (e.g. ASP.NET).
        // Swap for BasicConverter if single-threaded use is guaranteed.
        _converter = new SynchronizedConverter(new PdfTools());
    }

    /// <summary>
    /// Converts the rendered HTML file at <paramref name="htmlFilePath"/> into a PDF
    /// and writes it to the Output directory.
    /// </summary>
    /// <param name="htmlFilePath">Path to the fully rendered HTML file.</param>
    /// <param name="fileOutputName">Desired PDF file name (extension is optional).</param>
    /// <returns>Full path to the generated PDF file.</returns>
    public Task<string> CreatePdfAsync(string htmlFilePath, string fileOutputName)
    {
        ArgumentException.ThrowIfNullOrEmpty(htmlFilePath);
        ArgumentException.ThrowIfNullOrEmpty(fileOutputName);

        var pdfFileName = fileOutputName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
            ? fileOutputName
            : $"{fileOutputName}.pdf";

        var outputPath = Path.Combine(_outputDirectory, pdfFileName);

        _logger.Information("Converting {HtmlFile} → {PdfFile}", htmlFilePath, outputPath);

        var document = BuildPdfDocument(htmlFilePath);

        byte[] pdfBytes = _converter.Convert(document);
        File.WriteAllBytes(outputPath, pdfBytes);

        _logger.Information("PDF written ({Bytes:N0} bytes) → {OutputPath}", pdfBytes.Length, outputPath);

        OpenFileWithDefaultApplication(outputPath);

        return Task.FromResult(outputPath);
    }

    /// <summary>
    /// Builds the DinkToPdf document descriptor from a rendered HTML file path.
    /// Adjust <see cref="GlobalSettings"/> and <see cref="ObjectSettings"/> here
    /// to control paper size, margins, headers/footers, encoding, etc.
    /// </summary>
    private static HtmlToPdfDocument BuildPdfDocument(string htmlFilePath)
    {
        // Use a file:// URI so relative CSS/image assets in the template resolve correctly
        var fileUri = new Uri(Path.GetFullPath(htmlFilePath)).AbsoluteUri;

        return new HtmlToPdfDocument
        {
            GlobalSettings = new GlobalSettings
            {
                ColorMode   = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize   = PaperKind.A4,
                Margins     = new MarginSettings
                {
                    Top    = 10,
                    Bottom = 10,
                    Left   = 10,
                    Right  = 10,
                    Unit   = Unit.Millimeters,
                },
                // Leaving Out empty causes Convert() to return bytes rather than write the file itself
                Out = string.Empty,
            },
            Objects =
            {
                new ObjectSettings
                {
                    Page         = fileUri,
                    WebSettings  = new WebSettings { DefaultEncoding = "utf-8" },
                    LoadSettings = new LoadSettings { BlockLocalFileAccess = false },
                }
            }
        };
    }

    private static void OpenFileWithDefaultApplication(string filePath)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
        {
            UseShellExecute = true
        });
    }

    private void CreateOutputDirectory()
    {
        if (!Directory.Exists(_outputDirectory))
            Directory.CreateDirectory(_outputDirectory);
    }
}
