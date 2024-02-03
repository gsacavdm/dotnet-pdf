using System;
using System.Collections.Generic;
using System.IO;

using CommandLine;

using PdfParser.Documents;
using PdfParser.TextProcessors;

namespace PdfParser;

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

        var logger = new Logger(options.Verbose);

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

        var textProcessors = new List<ITextProcessor>
        {
            new CignaClaimProcessor(),
            new CignaWebClaimProcessor(),
            new CignaMoreInfoNeededClaimProcessor(),
            new PremeraClaimProcessor(),
            new RegenceClaimProcessor(),
            new VspClaimProcessor(),
            new TruistStatementProcessor(),
            new UhcClaimProcessor()
        };

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
                        logger.Log($"Parser found, using {textProcessor.GetType().Name}");
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
                logger.Log($"Exception processing ${filePath}");
                logger.Log(ex);
            }
        }
    }
}
