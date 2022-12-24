using System;
using System.Collections.Generic;
using System.IO;

using CommandLine;

using Sc.Pdf.Documents;
using Sc.Pdf.TextProcessors;

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
            filePaths.AddRange(Directory.EnumerateFiles(options.PdfDirectoryPath, options.PdfDirectoryFilePattern ?? "*.pdf"));
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

        List<ITextProcessor> textProcessors = new();
        textProcessors.Add(new CignaClaimProcessor());
        textProcessors.Add(new CignaMoreInfoNeededClaimProcessor());
        textProcessors.Add(new PremeraClaimProcessor());
        textProcessors.Add(new RegenceClaimProcessor());
        textProcessors.Add(new VspClaimProcessor());

        var hasWrittenCsvHeaders = false;
        foreach (var filePath in filePaths)
        {
            try
            {
                var text = PdfParsers.PdfParser.Parse(filePath);

                if (options.Mode.Equals("Simple", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var t in text) { Console.WriteLine(t); }
                    return;
                }

                IDocument document = null;
                foreach (var textProcessor in textProcessors)
                {
                    if (textProcessor.TryParse(text, out document))
                    {
                        // No need to keep trying processors
                        break;
                    }
                }

                if (document == null || !document.IsValid)
                {
                    throw new InvalidOperationException("No valid processor found.");
                }

                var fileName = Path.GetFileName(filePath);
                document.SourceFileName = fileName;

                if (options.Mode.Equals("Move", StringComparison.OrdinalIgnoreCase))
                {
                    var directoryName = Path.GetDirectoryName(filePath);
                    var newFileName = document.StandardFileName;

                    if (!document.IsValid || string.IsNullOrEmpty(newFileName))
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
                else if (options.Mode.Equals("Csv", StringComparison.OrdinalIgnoreCase))
                {
                    if (!hasWrittenCsvHeaders)
                    {
                        document.WriteCsvHeader();
                    }
                    document.WriteCsv();
                    hasWrittenCsvHeaders = true;
                }
                else
                {
                    // ToDo: Change Mode to an enum and fail earlier if it doesn't match the expected values
                }
            }
            catch (Exception ex)
            {
                if (options.Verbose)
                {
                    Console.WriteLine($"Exception processing ${filePath}");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
