﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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

        var text = ImportPdf(options.PdfPath);

        switch (options.Mode)
        {
            case "Regence":
                var regenceClaim = ExtractRegenceFields(text);
                regenceClaim.WriteLine();
                break;
            default:
                foreach (var t in text) { Console.WriteLine(t); }
                break;

        }

        //
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

    public static string ExtractField(IEnumerable<string> textRows, string searchText)
    {
        var textRow = textRows.FirstOrDefault(s => s.StartsWith(searchText, StringComparison.InvariantCulture));

        if (textRow == null)
        {
            return string.Empty;
        }

        textRow = textRow.Replace(searchText, "").Trim();
        return textRow;
    }

    public static DateTime? ExtractDate(IEnumerable<string> textRows, string searchText)
    {
        var couldParse = DateTime.TryParse(ExtractField(textRows, searchText), out var dateTime);
        return couldParse ? dateTime : null;
    }

    public static double? ExtractDouble(IEnumerable<string> textRows, string searchText)
    {
        var doubleString = ExtractField(textRows, searchText);
        doubleString = doubleString.Replace("$", "").Replace(",", "");
        var couldParse = double.TryParse(doubleString, out var doubleVal);
        return couldParse ? doubleVal : null;
    }

    public static RegenceModel ExtractRegenceFields(IEnumerable<string> text)
    {
        var model = new RegenceModel
        {
            ProviderName = ExtractField(text, "Provider name"),
            DateOfService = ExtractDate(text, "Date of service"),
            DateProcessed = ExtractDate(text, "Date processed"),
            PharmacyName = ExtractField(text, "Pharmacy name"),
            DateOfFill = ExtractDate(text, "Date of fill"),
            MedicationName = ExtractField(text, "Medication name").Replace("/", "-"),
            PrescriberName = ExtractField(text, "Prescriber name"),
            ClaimNumber = ExtractField(text, "Claim number"),
            AmountBilled = ExtractDouble(text, "Amount billed"),
            DiscountedRate = ExtractDouble(text, "Your discounted rate"),
            AmountPaid = ExtractDouble(text, "Amount we paid"),
            AmountYouOwe = ExtractDouble(text, "Amount you owe")
        };

        return model;
    }
}
