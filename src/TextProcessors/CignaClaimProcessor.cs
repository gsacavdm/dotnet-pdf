using System;
using System.Collections.Generic;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;
using Sc.Pdf.TextProcessors;

namespace Sc.Pdf.TextProcessors;

public class CignaClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var cignaClaim = new CignaClaim
        {
            SourceText = text
        };

        try
        {
            cignaClaim.ClaimNumber = text.ExtractFieldNextLineByEquals("Claim # / ID").Split(" ")[0];
            cignaClaim.ProviderName = text.ExtractFieldByStartsWith("for services provided by");

            var dateOfServiceLine = text.ExtractFieldNextLineByEquals("Service date");
            if (string.IsNullOrEmpty(dateOfServiceLine))
            {
                dateOfServiceLine = text.ExtractFieldNextLineByEquals("Service dates");
            }
            cignaClaim.DateOfService = dateOfServiceLine.Split(" - ")[0].ParseDate();

            cignaClaim.AmountBilled = text.ExtractFieldNextLineByEquals("Amount Billed").ParseDouble();
            cignaClaim.Discount = text.ExtractFieldNextLineByEquals("Discount").ParseDouble();
            cignaClaim.AmountPaid = text.ExtractFieldNextLineByEquals("paid").ParseDouble();
            cignaClaim.AmountYouOwe = text.ExtractFieldNextLineByEquals("What I Owe").ParseDouble();

            var dateProcessedLine = text.ExtractFieldByStartsWith("Cigna received this claim on");
            var dateProcessedParts = dateProcessedLine.Split("processed it on ");
            cignaClaim.DateProcessed = dateProcessedParts.Length > 1 ? dateProcessedParts[1].Replace(".", "").ParseDate() : null;
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            cignaClaim.ParseException = ex;
        }

        document = cignaClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
