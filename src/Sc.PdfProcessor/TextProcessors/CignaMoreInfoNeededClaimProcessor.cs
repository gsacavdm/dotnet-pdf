using System;
using System.Collections.Generic;
using Sc.PdfProcessor.Documents;
using Sc.PdfProcessor.Extensions;
using Sc.PdfProcessor.TextProcessors;

namespace Sc.PdfProcessor.TextProcessors;

public class CignaMoreInfoNeededClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var cignaClaimMoreInfo = new CignaMoreInfoNeededClaim
        {
            SourceText = text
        };

        try
        {
            cignaClaimMoreInfo.ClaimNumber = text.ExtractFieldByStartsWith("Claim Number:");
            cignaClaimMoreInfo.ProviderName = text.ExtractFieldByStartsWith("Provider:");
            cignaClaimMoreInfo.MemberName = text.ExtractFieldByStartsWith("Patient:");
            cignaClaimMoreInfo.DateOfService = text.ExtractFieldByStartsWith("Date of Service:").Split(" - ")[0].ParseDate();
            cignaClaimMoreInfo.DateProcessed = text.ExtractFieldPreviousLineByStartsWith("Subscriber:").ParseDate();
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
