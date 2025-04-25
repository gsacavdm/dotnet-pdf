using System;
using System.Collections.Generic;
using PdfParser.Documents;
using PdfParser.Extensions;

namespace PdfParser.TextProcessors;

public class UhcClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var uhcClaim = new UhcClaim
        {
            SourceText = text
        };

        try
        {
            uhcClaim.ClaimNumber = text.ExtractFieldByStartsWith("Claim number: ");

            uhcClaim.ProviderName = text.ExtractFieldByStartsWith("Provider: ").SanitizeAndTitleCase();
            // Sometimes this line is parsed together with the "Patient Account Number" line
            if (uhcClaim.ProviderName.Contains("Patient Account Number"))
            {
                uhcClaim.ProviderName = uhcClaim.ProviderName.Split("Patient Account Number")[0].Trim();
            }

            uhcClaim.MemberName = text.ExtractFieldByStartsWith("Claim detail for ").SanitizeAndTitleCase();

            uhcClaim.DateOfService = text.ExtractFieldNextLineByEquals("Services in this statement occurred")
                .Replace(" between ", "")
                .Split(" - ")[0]
                .ParseDate();
            uhcClaim.DateProcessed = text.ExtractFieldNextLineByStartsWith("Page 1 of").ParseDate();

            uhcClaim.AmountBilled = text.ExtractFieldByStartsWith("Provider billed").ParseDouble();
            uhcClaim.AmountYouOwe = text.ExtractFieldByStartsWith("Your total amount owed").ParseDouble();

            var totalLine = text.ExtractFieldByStartsWith("Total amount");
            var totalParts = totalLine.Split(" ");
            if (totalParts.Length == 9)
            {
                uhcClaim.Discount = totalParts[1].ParseDouble();
                uhcClaim.AllowedAmount = totalParts[2].ParseDouble();
                uhcClaim.AmountPaid = totalParts[3].ParseDouble();
                uhcClaim.Deductible = totalParts[4].ParseDouble();
                uhcClaim.Copay = totalParts[5].ParseDouble();
                uhcClaim.Coinsurance = totalParts[6].ParseDouble();
                uhcClaim.AmountNotCovered = totalParts[7].ParseDouble();
            }

            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            uhcClaim.ParseException = ex;
        }

        document = uhcClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
