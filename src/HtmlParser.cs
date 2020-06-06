using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HtmlToPdfCore
{
    /// <summary>
    /// Replace html place holders with the values provided
    /// </summary>
    public class HtmlParser
    {
        /// <summary>
        /// Replaces the {{placeholders}} of the html template with the dictionary values
        /// </summary>
        /// <param name="htmlFilePath">path to html template file</param>
        /// <param name="keyValues">Dictionary of values to replace in the template</param>
        /// <param name="outputHtmlFileName"> output html file name to convert to pdf</param>
        public void Parse(string htmlFilePath, Dictionary<string, string> keyValues, string outputHtmlFileName)
        {
            string htmlFile = File.ReadAllText(htmlFilePath);
            foreach (var keyValue in keyValues)
            {
                htmlFile = htmlFile.Replace("{{" + keyValue.Key + "}}", keyValue.Value);
            }
            File.WriteAllText(outputHtmlFileName, htmlFile);
        }

        public Dictionary<string, string> BuildDictionary()
        {
            var keyValues = new Dictionary<string, string>();
            var companyName = "TEST COMPANY (PTY) LTD";
            var companyNumber = "CC 101";
            var to = "Test Client";
            var attention = "John Doe";
            var date = DateTime.Now.ToShortDateString();
            var quoteNumber = "12345";
            var tableBody = BuildQuoteTableBody(BuildFakeTable());
            var conditions = BuildConditionsOnNewLine(BuildFakeConditions());
            var user = "Test Person";
            var watermark = ""; //"Watermark Here";

            keyValues.Add("CompanyName", companyName);
            keyValues.Add("CompanyNumber", companyNumber);
            keyValues.Add("To", to);
            keyValues.Add("Attention", attention);
            keyValues.Add("Date", date);
            keyValues.Add("QuoteNumber", quoteNumber);
            keyValues.Add("TableBody", tableBody);
            keyValues.Add("Conditions", conditions);
            keyValues.Add("User", user);
            keyValues.Add("Watermark", watermark);
            return keyValues;
        }


        private string BuildQuoteTableBody(List<QuoteTableObject> quoteTable)
        {
            var result = "";
            foreach (var item in quoteTable)
            {
                result += "<tr>" +
                                $"<td>{item.Description}</td>" +
                                $"<td>{item.Quantity}</td>" +
                                $"<td>{item.Cost}</td>" +
                            "</tr>";
            }
            return result;
        }

        private string BuildConditions(string input)
        {
            var strlist = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var result = "";
            foreach (var item in strlist)
            {
                result += $@"{item} <br/>";
            }
            return result;
        }
        private string BuildConditionsOnNewLine(string input)
        {
            var result = input.Replace(Environment.NewLine, $@" <br />");
            return result;
        }

        private List<QuoteTableObject> BuildFakeTable()
        {
            var result = new List<QuoteTableObject>()
            {
                {new QuoteTableObject("This is a test description 1", "1", "R200.00") },
                {new QuoteTableObject("This is a test description 2", "2", "R400.00") },
                {new QuoteTableObject("This is a test description 3", "3", "R800.00") },
                {new QuoteTableObject("", "Total Excl. VAT", "R1400.00") },
            };

            return result;
        }
        private string BuildFakeConditions()
        {
            var result = new StringBuilder();
            result.AppendLine("- This quotation is valid for a period of 10 days from the date above");
            result.AppendLine("- Goods supplied or repaired remain the property until paid for in full");
            result.AppendLine("-These prices do not include shipping, customs clearance or VAT charges.");
            result.AppendLine("- E. & O.E. (Errors and omissions excepted).");
            return result.ToString();
        }
    }
}
