using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace HtmlToPdfCore
{
    /// <summary>
    /// Convert Html to pdf using https://wkhtmltopdf.org/
    /// </summary>
    public class HtmlToPdfService
    {
        private readonly string _path;
        private readonly string _outputDirctory;

        public HtmlToPdfService()
        {
            _path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _outputDirctory = $@"{_path}\Output\";
            CreateOututDirectory();
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
                Console.WriteLine("Wkhtmltopdf Output:" );
                using (var process = Process.Start(pi))
                {                   
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    string err = process.StandardError.ReadToEnd();
                    Console.WriteLine(err);
                    process.WaitForExit();
                    Console.WriteLine(process.ExitCode);
                }                

                OpenFileWithDefaultApplication(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating pdf {ex.Message}");                
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

        private void CreateOututDirectory()
        {
            var directory = new DirectoryInfo(_outputDirctory);
            if (directory.Exists)
                return;

            directory.Create();
        }
    }
}
