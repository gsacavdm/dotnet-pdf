using System.Collections.Generic;
using System.Text;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace PdfParser.PdfParsers;

public class PdfParser
{
    public static IEnumerable<string> Parse(string pdfPath)
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
