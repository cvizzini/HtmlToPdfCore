using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace HtmlToPdfCore;

/// <summary>
/// Convert Html to pdf using https://wkhtmltopdf.org/
/// </summary>
public class HtmlToPdfService
{
    private readonly string _path;
    private readonly string _outputDirctory;
    private readonly ILogger _logger;

    public HtmlToPdfService(ILogger logger)
    {
        _logger = logger.ForContext<HtmlToPdfService>();
        _path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        _outputDirctory = $@"{_path}\Output\";
        CreateOutputDirectory();
    }

    public void CreatePdf(string htmlFilePath, string fileOutputName)
    {
        try
        {
            // Path to the wkhtmltopdf.exe
            var pi = new ProcessStartInfo(@$"{_path}\wkhtmltopdf\Windows\wkhtmltopdf.exe");
            pi.CreateNoWindow = true;
            pi.UseShellExecute = false;
            pi.WorkingDirectory = _path;
            pi.RedirectStandardOutput = true;
            pi.RedirectStandardError = true;

            var htmlFile = new FileInfo(htmlFilePath);

            //Ensure output extension has pdf
            var pdfFileOutputName = fileOutputName.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase) ?
                fileOutputName : $"{fileOutputName}.pdf";

            var outputPath = (@$"{_outputDirctory}{pdfFileOutputName}");
            pi.WorkingDirectory = htmlFile.Directory.FullName;

            pi.Arguments = @$"{htmlFile.Name} {outputPath}";
            _logger.Information("Wkhtmltopdf Output:");
            using (var process = Process.Start(pi))
            {
                string output = process.StandardOutput.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(output))
                    _logger.Debug("{Output}", output);
                string err = process.StandardError.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(err))
                    _logger.Warning("{StdErr}", err);
                process.WaitForExit();
                _logger.Information("wkhtmltopdf exited with code {ExitCode}", process.ExitCode);
            }

            OpenFileWithDefaultApplication(outputPath);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating PDF");
        }
    }

    private static void OpenFileWithDefaultApplication(string pdfFileOutputName)
    {
        var p = new Process();
        p.StartInfo = new ProcessStartInfo(pdfFileOutputName)
        {
            UseShellExecute = true
        };
        p.Start();
        p.WaitForExit();
    }

    private void CreateOutputDirectory()
    {
        var directory = new DirectoryInfo(_outputDirctory);
        if (directory.Exists)
            return;

        directory.Create();
    }
}