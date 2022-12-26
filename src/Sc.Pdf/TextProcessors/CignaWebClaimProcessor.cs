using System;
using System.Collections.Generic;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.TextProcessors;

public class CignaWebClaimProcessor : ITextProcessor
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
            cignaClaim.ClaimNumber = text.ExtractFieldByRegex("Medical Claim #(?<value>[0-9]+)");
            cignaClaim.ProviderName = text.ExtractFieldNextLineByEquals("Services Provided By:");
            cignaClaim.MemberName = text.ExtractFieldByRegex("(?<value>[a-zA-Z]+)’s Medical Claim");

            var serviceDateLine = text.ExtractFieldByStartsWith("Service Date");
            var serviceDateParts = serviceDateLine.Split(" ");
            if (serviceDateParts.Length == 11)
            {
                cignaClaim.DateOfService = serviceDateParts[0].ParseDate();
                cignaClaim.DateProcessed = serviceDateParts[6].ParseDate();
            }

            var totalLine = text.ExtractFieldByStartsWith("Totals");
            var totalParts = totalLine.Split(" ");
            if (totalParts.Length == 10)
            {
                cignaClaim.AmountBilled = totalParts[0].ParseDouble();
                cignaClaim.AllowedAmount = totalParts[2].ParseDouble();
                // cignaClaim.Discount = ??

                cignaClaim.AmountPaid = totalParts[3].ParseDouble();

                cignaClaim.AmountNotCovered = totalParts[4].ParseDouble();
                cignaClaim.Deductible = totalParts[5].ParseDouble();
                cignaClaim.Copay = totalParts[6].ParseDouble();
                cignaClaim.Coinsurance = totalParts[7].ParseDouble();

                cignaClaim.AmountYouOwe = totalParts[9].ParseDouble();
            }
            cignaClaim.ClaimType = CignaClaim.CignaClaimType.Web;
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
