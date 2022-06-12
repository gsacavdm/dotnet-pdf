using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CommandLine;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

using Sc.Pdf.Models;
namespace Sc.Pdf;

public class Program
{
    public static void Main(string[] args)
    {
        Options options = null;
        Parser.Default.ParseArguments<Options>(args)
          .WithParsed(o =>
          {
              options = o;
          });

        if (options == null)
        {
            return;
        }

        // TODO: Convert to option
        var overwriteOnMove = false;

        var filePaths = new List<string>();
        if (!string.IsNullOrEmpty(options.PdfDirectoryPath))
        {
            filePaths.AddRange(Directory.EnumerateFiles(options.PdfDirectoryPath, "*.pdf"));
        }
        else if (!string.IsNullOrEmpty(options.PdfFilePath))
        {
            filePaths.Add(options.PdfFilePath);
        }
        else
        {
            Console.WriteLine("Please specify either -f or -d");
            return;
        }

        foreach (var filePath in filePaths)
        {
            var text = ImportPdf(filePath);

            if (options.Mode.StartsWith("Regence", false, CultureInfo.InvariantCulture))
            {
                var regenceClaim = ExtractRegenceFields(text);
                if (options.Mode == "RegenceMove")
                {
                    var directoryName = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var newFileName = regenceClaim.FileName;

                    if (string.IsNullOrEmpty(newFileName))
                    {
                        Console.WriteLine($"Couldn't extract {fileName}, skipping...");
                        continue;
                    }

                    var newFilePath = Path.Combine(directoryName, newFileName);

                    Console.WriteLine($"Moving {fileName} to {newFileName}");
                    try
                    {
                        File.Move(filePath, newFilePath, overwriteOnMove);
                    }
                    catch (IOException ioex)
                    {
                        Console.WriteLine($"Unable to move {fileName} due to exception \"{ioex.Message}\"");
                    }
                }
                else
                {
                    regenceClaim.WriteLine();
                }
            }
            else if (options.Mode.StartsWith("Premera", false, CultureInfo.InvariantCulture))
            {
                var premeraClaim = ExtractPremeraFields(text);
                if (options.Mode == "PremeraMove")
                {
                    var directoryName = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var newFileName = premeraClaim.FileName;

                    var newFilePath = Path.Combine(directoryName, newFileName);

                    Console.WriteLine($"Moving {fileName} to {newFileName}");
                    try
                    {
                        File.Move(filePath, newFilePath, overwriteOnMove);
                    }
                    catch (IOException ioex)
                    {
                        Console.WriteLine($"Unable to move {fileName} due to exception \"{ioex.Message}\"");
                    }
                }
                else
                {
                    premeraClaim.WriteLine();
                }
            }
            else if (options.Mode.StartsWith("Cigna", false, CultureInfo.InvariantCulture))
            {
                var premeraClaim = ExtractCignaFields(text);
                if (options.Mode == "CignaMove")
                {
                    var directoryName = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var newFileName = premeraClaim.FileName;

                    var newFilePath = Path.Combine(directoryName, newFileName);

                    Console.WriteLine($"Moving {fileName} to {newFileName}");
                    try
                    {
                        File.Move(filePath, newFilePath, overwriteOnMove);
                    }
                    catch (IOException ioex)
                    {
                        Console.WriteLine($"Unable to move {fileName} due to exception \"{ioex.Message}\"");
                    }
                }
                else
                {
                    premeraClaim.WriteLine();
                }
            }
            else
            {
                foreach (var t in text) { Console.WriteLine(t); }
            }
        }
    }

    public static IEnumerable<string> ImportPdf(string pdfPath)
    {
        List<string> text = new();

        using var reader = new PdfReader(pdfPath);
        using var document = new PdfDocument(reader);
        for (var pageNumber = 1; pageNumber <= document.GetNumberOfPages(); pageNumber++)
        {
            var page = document.GetPage(pageNumber);
            var currentText = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
            var pageText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));

            var textRows = pageText.Split("\n");
            text.AddRange(textRows);
        }

        return text;
    }

    public static string ExtractFieldByPosition(IEnumerable<string> textRows, int position)
    {
        var textRow = textRows.ElementAtOrDefault(position);

        return textRow ?? string.Empty;
    }

    public static string ExtractFieldNextLineByEquals(IEnumerable<string> textRows, string searchText)
    {
        var searchLine = textRows.FirstOrDefault(line => line.Equals(searchText, StringComparison.Ordinal));
        if (searchLine == null)
        {
            return string.Empty;
        }

        var searchPosition = Array.IndexOf(textRows.ToArray(), searchLine);
        var textRow = textRows.ElementAtOrDefault(searchPosition + 1);
        return textRow ?? string.Empty;
    }

    public static DateTime? ParseDate(string dateText)
    {
        var couldParse = DateTime.TryParse(dateText, out var dateTime);
        return couldParse ? dateTime : null;
    }

    public static string ExtractFieldByStartsWith(IEnumerable<string> textRows, string searchText)
    {
        var textRow = textRows.FirstOrDefault(s => s.StartsWith(searchText, StringComparison.InvariantCulture));

        if (textRow == null)
        {
            return string.Empty;
        }

        textRow = textRow.Replace(searchText, "").Trim();
        return textRow;
    }

    public static DateTime? ExtractDateByStartsWith(IEnumerable<string> textRows, string searchText)
    {
        var couldParse = DateTime.TryParse(ExtractFieldByStartsWith(textRows, searchText), out var dateTime);
        return couldParse ? dateTime : null;
    }

    public static double? ParseDouble(string doubleString)
    {
        doubleString = doubleString.Replace("$", "").Replace(",", "");
        var couldParse = double.TryParse(doubleString, out var doubleVal);
        return couldParse ? doubleVal : null;
    }

    public static RegenceClaim ExtractRegenceFields(IEnumerable<string> text)
    {
        var model = new RegenceClaim
        {
            ProviderName = ExtractFieldByStartsWith(text, "Provider name"),
            DateOfService = ParseDate(ExtractFieldByStartsWith(text, "Date of service")),
            DateProcessed = ParseDate(ExtractFieldByStartsWith(text, "Date processed")),
            PharmacyName = ExtractFieldByStartsWith(text, "Pharmacy name"),
            NdcNumber = ExtractFieldByStartsWith(text, "NDC number"),
            PrescriptionNumber = ExtractFieldByStartsWith(text, "Prescription number"),
            DateOfFill = ExtractDateByStartsWith(text, "Date of fill"),
            MedicationName = ExtractFieldByStartsWith(text, "Medication name").Replace("/", "-"),
            PrescriberName = ExtractFieldByStartsWith(text, "Prescriber name"),
            ClaimNumber = ExtractFieldByStartsWith(text, "Claim number"),
            AmountBilled = ParseDouble(ExtractFieldByStartsWith(text, "Amount billed")),
            DiscountedRate = ParseDouble(ExtractFieldByStartsWith(text, "Your discounted rate")),
            AmountPaid = ParseDouble(ExtractFieldByStartsWith(text, "Amount we paid")),
            AmountYouOwe = ParseDouble(ExtractFieldByStartsWith(text, "Amount you owe"))
        };

        return model;
    }

    public static PremeraClaim ExtractPremeraFields(IEnumerable<string> text)
    {

        var model = new PremeraClaim();

        foreach (var line in text)
        {
            if (line.StartsWith("For services provided by", false, CultureInfo.InvariantCulture))
            {
                var tmp = line.Replace("For services provided by ", "");
                var parts = tmp.Split(" on ");

                if (parts.Length == 2)
                {
                    model.ProviderName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[0].ToLower(CultureInfo.InvariantCulture));
                    model.DateOfService = DateTime.TryParse(parts[1], out var date) ? date : null;
                }
            }
            else if (Regex.IsMatch(line, "[a-zA-Z]+ [0-9]{2}, [0-9]{4}") && model.DateProcessed == null)
            {
                model.DateProcessed = DateTime.TryParse(line, out var date) ? date : null;
            }
            else if (Regex.IsMatch(line, "Claim #"))
            {
                var parts = line.Split("# ");
                model.ClaimNumber = parts[1].Split(",")[0];
            }
        }

        return model;
    }

    public static CignaClaim ExtractCignaFields(IEnumerable<string> text)
    {

        var claimNumber = ExtractFieldNextLineByEquals(text, "Claim # / ID").Split(" ")[0];
        var providerName = ExtractFieldByStartsWith(text, "for services provided by");
        var dateOfService = ParseDate(ExtractFieldNextLineByEquals(text, "Service date"));
        var amountBilled = ParseDouble(ExtractFieldNextLineByEquals(text, "Amount Billed"));
        var discount = ParseDouble(ExtractFieldNextLineByEquals(text, "Discount"));
        var amountPaid = ParseDouble(ExtractFieldNextLineByEquals(text, "paid"));
        var amountYouOwe = ParseDouble(ExtractFieldNextLineByEquals(text, "What I Owe"));
        var dateProcessedLine = ExtractFieldByStartsWith(text, "Cigna received this claim on");
        var dateProcessed = ParseDate(dateProcessedLine.Split("processed it on ")[1].Replace(".", ""));

        var model = new CignaClaim
        {
            ClaimNumber = claimNumber,
            ProviderName = providerName,
            DateOfService = dateOfService,
            AmountBilled = amountBilled,
            Discount = discount,
            AmountPaid = amountPaid,
            AmountYouOwe = amountYouOwe,
            DateProcessed = dateProcessed
        };

        return model;
    }
}
