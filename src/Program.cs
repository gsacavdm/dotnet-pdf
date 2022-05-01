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
            Console.WriteLine("Please specify either -p or -d");
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
        for (var pageNumber = 1; pageNumber < document.GetNumberOfPages(); pageNumber++)
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

    public static double? ExtractDoubleByStartsWith(IEnumerable<string> textRows, string searchText)
    {
        var doubleString = ExtractFieldByStartsWith(textRows, searchText);
        doubleString = doubleString.Replace("$", "").Replace(",", "");
        var couldParse = double.TryParse(doubleString, out var doubleVal);
        return couldParse ? doubleVal : null;
    }

    public static RegenceClaim ExtractRegenceFields(IEnumerable<string> text)
    {
        var model = new RegenceClaim
        {
            ProviderName = ExtractFieldByStartsWith(text, "Provider name"),
            DateOfService = ExtractDateByStartsWith(text, "Date of service"),
            DateProcessed = ExtractDateByStartsWith(text, "Date processed"),
            PharmacyName = ExtractFieldByStartsWith(text, "Pharmacy name"),
            DateOfFill = ExtractDateByStartsWith(text, "Date of fill"),
            MedicationName = ExtractFieldByStartsWith(text, "Medication name").Replace("/", "-"),
            PrescriberName = ExtractFieldByStartsWith(text, "Prescriber name"),
            ClaimNumber = ExtractFieldByStartsWith(text, "Claim number"),
            AmountBilled = ExtractDoubleByStartsWith(text, "Amount billed"),
            DiscountedRate = ExtractDoubleByStartsWith(text, "Your discounted rate"),
            AmountPaid = ExtractDoubleByStartsWith(text, "Amount we paid"),
            AmountYouOwe = ExtractDoubleByStartsWith(text, "Amount you owe")
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
}
