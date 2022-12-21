using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using CommandLine;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Sc.Pdf.Extensions;
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

        var overwriteOnMove = options.Overwrite;

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
            try
            {
                var text = ImportPdf(filePath);

                IClaim claim;
                if (options.Mode.StartsWith("Regence", false, CultureInfo.InvariantCulture))
                {
                    claim = new RegenceClaim(text);
                }
                else if (options.Mode.StartsWith("Premera", false, CultureInfo.InvariantCulture))
                {
                    claim = new PremeraClaim(text);
                }
                else if (options.Mode.StartsWith("Cigna", false, CultureInfo.InvariantCulture))
                {
                    claim = new CignaClaim(text);
                }
                else if (options.Mode.StartsWith("Vsp", false, CultureInfo.InvariantCulture))
                {
                    claim = new VspClaim(text);
                }
                else
                {
                    foreach (var t in text) { Console.WriteLine(t); }
                    return;
                }

                if (options.Mode.EndsWith("Move", true, CultureInfo.InvariantCulture))
                {
                    var directoryName = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var newFileName = claim.FileName;

                    if (!claim.IsValid || string.IsNullOrEmpty(newFileName))
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
                    claim.WriteCsv();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception processing ${filePath}");
                Console.WriteLine(ex);
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
}
