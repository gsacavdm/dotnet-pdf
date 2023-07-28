using System;
using System.Collections.Generic;
using PdfParser.Documents;
using PdfParser.Extensions;
using PdfParser.TextProcessors;

namespace PdfParser.TextProcessors;

public class CignaMoreInfoNeededClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var cignaClaimMoreInfo = new CignaClaim
        {
            SourceText = text
        };

        try
        {
            cignaClaimMoreInfo.ClaimNumber = text.ExtractFieldByStartsWith("Claim Number:");
            cignaClaimMoreInfo.ProviderName = text.ExtractFieldByStartsWith("Provider:");
            cignaClaimMoreInfo.MemberName = text.ExtractFieldByStartsWith("Patient:");
            cignaClaimMoreInfo.DateOfService = text.ExtractFieldByStartsWith("Date of Service:")
                .Split(new string[] { " - ", " -" }, StringSplitOptions.None)[0]
                .ParseDate();
            cignaClaimMoreInfo.DateProcessed = text.ExtractFieldPreviousLineByStartsWith("Subscriber:").ParseDate();
            cignaClaimMoreInfo.ClaimType = CignaClaim.CignaClaimType.MoreInfoNeeded;
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            cignaClaimMoreInfo.ParseException = ex;
        }

        document = cignaClaimMoreInfo;
        return parsedSuccessfully && document.IsValid;
    }
}
