using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace HtmlToPdfCore;

/// <summary>
/// Converts an HTML file to PDF using the bundled wkhtmltopdf binary.
/// See: https://wkhtmltopdf.org/
/// </summary>
public class HtmlToPdfService
{
    private readonly string _basePath;

    // #3 - Fixed typo: _outputDirctory → _outputDirectory
    private readonly string _outputDirectory;
    private readonly ILogger _logger;

    public HtmlToPdfService(ILogger logger)
    {
        _logger = logger.ForContext<HtmlToPdfService>();

        // #2 - Added null guard with descriptive error instead of silent null-forgiving
        _basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
            ?? throw new InvalidOperationException("Could not resolve the entry assembly location.");

        _outputDirectory = Path.Combine(_basePath, "Output");
        CreateOutputDirectory();
    }

    // #4 - Made async; reads stdout/stderr with async methods
    public async Task CreatePdfAsync(string htmlFilePath, string fileOutputName)
    {
        try
        {
            var htmlFile = new FileInfo(htmlFilePath);

            // Ensure output has a .pdf extension
            var pdfFileName = fileOutputName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
                ? fileOutputName
                : $"{fileOutputName}.pdf";

            var outputPath = Path.Combine(_outputDirectory, pdfFileName);

            // #1 - Object initialiser used consistently; WorkingDirectory set once
            var startInfo = new ProcessStartInfo(Path.Combine(_basePath, "wkhtmltopdf", "Windows", "wkhtmltopdf.exe"))
            {
                CreateNoWindow          = true,
                UseShellExecute         = false,
                WorkingDirectory        = htmlFile.Directory!.FullName,
                RedirectStandardOutput  = true,
                RedirectStandardError   = true,
                Arguments               = $"{htmlFile.Name} {outputPath}"
            };

            _logger.Information("wkhtmltopdf starting...");

            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Failed to start wkhtmltopdf process.");

            // #4 - Async reads prevent deadlocks on large output buffers
            string output = await process.StandardOutput.ReadToEndAsync();
            string error  = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(output))
                _logger.Debug("{Output}", output);

            if (!string.IsNullOrWhiteSpace(error))
                _logger.Warning("{StdErr}", error);

            _logger.Information("wkhtmltopdf exited with code {ExitCode}", process.ExitCode);

            // #8 - Fire-and-forget: open the file without blocking until the viewer is closed
            OpenFileWithDefaultApplication(outputPath);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating PDF");
        }
    }

    // #8 - Removed WaitForExit(); we don't want to block until the user closes their PDF viewer
    private static void OpenFileWithDefaultApplication(string filePath)
    {
        Process.Start(new ProcessStartInfo(filePath)
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
