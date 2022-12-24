using System;
using System.Collections.Generic;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;
using Sc.Pdf.TextProcessors;

namespace Sc.Pdf.TextProcessors;

public class VspClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var vspClaim = new VspClaim
        {
            SourceText = text
        };

        try
        {
            vspClaim.ClaimNumber = text.ExtractFieldByStartsWith("Claim #:").Split(" ")[0];
            vspClaim.DateOfService = text.ExtractFieldByStartsWith("Date of Service:").ParseDate();
            vspClaim.DateOfNotice = text.ExtractFieldByStartsWith("Date of Notice:").ParseDate();
            vspClaim.ProviderName = text.ExtractFieldByStartsWith("Provider:");
            vspClaim.MemberName = text.ExtractFieldByStartsWith("Name:").Split(" Case Number:")[0];
            vspClaim.AmountBilled = text.ExtractFieldNextLineByEquals("Billed:").ParseDouble();
            vspClaim.AmountPaid = text.ExtractFieldNextLineByEquals("Paid:").ParseDouble();
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            vspClaim.ParseException = ex;
        }

        document = vspClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
