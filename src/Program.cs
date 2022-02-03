using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using CommandLine;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace dotnet_pdf 
{
  class Program
  {
    static void Main(string[] args)
    {
      Options options = null;
      Parser.Default.ParseArguments<Options>(args)
        .WithParsed<Options>(o =>
        {
          options = o;
        });

      if (options == null) throw new ArgumentNullException(nameof(options));

      var text = ImportPdf(options.PdfPath);

      foreach(var t in text) { Console.WriteLine(t); }
      
      //ExtractRegenceFields(text);
    } 

    public static IEnumerable<string> ImportPdf(string pdfPath)
    {
      List<string> text = new();

      using var reader = new PdfReader(pdfPath);
      using var document = new PdfDocument(reader);
      for(var pageNumber = 1; pageNumber < document.GetNumberOfPages(); pageNumber++)
      {
      var page = document.GetPage(pageNumber);
      var currentText = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
      var pageText = Encoding.UTF8.GetString(System.Text.ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));

      var textRows = pageText.Split("\n");
      text.AddRange(textRows);
      }

      return text;
    }

    public static string ExtractField(IEnumerable<string> textRows, string searchText)
    {
      var textRow = textRows.FirstOrDefault(s => s.StartsWith(searchText));

      if (textRow == null) return string.Empty;

      textRow = textRow.Replace(searchText, "").Trim(); 
      return textRow;
    }

    public static string ExtractDate(IEnumerable<string> textRows, string searchText)
    {
      var couldParse = DateTime.TryParse(ExtractField(textRows, searchText), out var dateTime);

      return couldParse ? dateTime.ToString("yyy-MM-dd") : string.Empty;
    }

    public static string MapProvider(string providerName)
    {
      if (providerName.Contains(","))
      {
      var nameParts = providerName.Split(",");
      providerName = nameParts[1].Trim() + " " + nameParts[0].Trim();
      }

      providerName = providerName.Replace(",","");
      providerName = providerName.Replace(".","");
      providerName = providerName.Replace(" Inc ", " ");

      // Remove middle names (e.g. Jason J Lukas -> remove the J)
      providerName = String.Join(" ", providerName.Split(" ").Where(s => s.Length > 1));
      
      var mappedProviders = new Dictionary<string, string>
      {    
      {"Jason Lukas", "Physiatrist"}
      // ... add more ...
      };

      mappedProviders.TryGetValue(providerName, out var mappedProvider);

      return mappedProvider ?? providerName;
    }

    public static void ExtractRegenceFields(IEnumerable<string> text)
    {
      var providerName = ExtractField(text, "Provider name");

      var dateOfService = ExtractDate(text, "Date of service");
      var dateProcessed = ExtractDate(text, "Date processed");
      var pharmacyName = ExtractField(text, "Pharmacy name");
      var dateOfFill = ExtractDate(text, "Date of fill");
      var medicationName = ExtractField(text, "Medication name").Replace("/","-");
      var prescriberName = ExtractField(text, "Prescriber name");
      var claimNumber = ExtractField(text, "Claim number");
      var amountBilled = ExtractField(text, "Amount billed");
      var discountedRate = ExtractField(text, "Your discounted rate");
      var amountPaid = ExtractField(text, "Amount we paid");
      var amountYouOwe = ExtractField(text, "Amount you owe");

      Console.WriteLine("==============");

      Console.WriteLine($"Provider name: {providerName}");
      Console.WriteLine($"Date of service: {dateOfService}");
      Console.WriteLine($"Pharmacy name: {pharmacyName}");
      Console.WriteLine($"Date of fill: {dateOfFill}");
      Console.WriteLine($"Medication name: {medicationName}");
      Console.WriteLine($"Prescriber name: {prescriberName}");
      Console.WriteLine($"Claim number: {claimNumber}");
      Console.WriteLine($"Amount billed: {amountBilled}");
      Console.WriteLine($"Discounted rate: {discountedRate}");
      Console.WriteLine($"Amount paid: {amountPaid}");
      Console.WriteLine($"Amount you owe: {amountYouOwe}");

      if (!string.IsNullOrEmpty(pharmacyName))
      {
      dateOfService = dateOfFill;
      dateProcessed = dateOfFill;
      providerName = prescriberName;
      }

      providerName = MapProvider(providerName);

      if (!string.IsNullOrEmpty(pharmacyName))
      {
      providerName = $"{providerName} {pharmacyName} {medicationName}";
      }

      providerName = $"{dateOfService} {providerName} Claim Regence {dateProcessed}.pdf";

      Console.WriteLine("==============");

      Console.WriteLine($"Final provider name: {providerName}");
    }
  }
}
