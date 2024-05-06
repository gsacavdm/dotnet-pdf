using System;
using System.Collections.Generic;
using PdfParser.Documents;
using PdfParser.Extensions;

namespace PdfParser.TextProcessors;

public class RegenceEobProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var regenceEob = new RegenceEob
        {
            SourceText = text
        };

        try
        {
            parsedSuccessfully = true;
            regenceEob.PrintDate = text.ExtractFieldByStartsWith("Print Date: ").ParseDate();
        }
        catch (Exception ex)
        {
            regenceEob.ParseException = ex;
        }

        document = regenceEob;
        return parsedSuccessfully && document.IsValid;
    }
}
