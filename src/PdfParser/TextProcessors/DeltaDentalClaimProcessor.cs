using System;
using System.Collections.Generic;
using System.Linq;
using PdfParser.Documents;
using PdfParser.Extensions;

namespace PdfParser.TextProcessors;

public class DeltaDentalClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var deltaDentalClaim = new DeltaDentalClaim
        {
            SourceText = text
        };

        try
        {
            deltaDentalClaim.ClaimNumber = text.ExtractFieldByStartsWith("# 1 Claim number:");
            deltaDentalClaim.DateOfService = text.ExtractFieldByStartsWith("Date of service:").ParseDate();
            deltaDentalClaim.DateProcessed = text.ExtractFieldByStartsWith("Date:").ParseDate();
            deltaDentalClaim.ProviderName = text.ExtractFieldByStartsWith("  Treating provider:").RemoveRedundantSpaces().ToTitleCase();
            deltaDentalClaim.MemberName = text.ExtractFieldByStartsWith("Claim for ").RemoveRedundantSpaces().ToTitleCase();

            var total = text.ExtractFieldByStartsWith("Claim total for");
            var totalParts = total.Split(" ")
                .Select(x => x.ParseDouble())
                .Where(x => x.HasValue)
                .ToArray();

            var i = 0;
            deltaDentalClaim.SubmittedFee = totalParts[i++];
            deltaDentalClaim.AcceptedFee = totalParts[i++];
            deltaDentalClaim.MaximumContractAllowance = totalParts[i++];
            deltaDentalClaim.AmountAppliedToDeductible = totalParts[i++];
            deltaDentalClaim.PaidByAnotherPlan = totalParts[i++];

            // Contract benefit level is optional
            if (totalParts.Length == 8)
            {
                deltaDentalClaim.ContactBenefitLevel = totalParts[i++];
            }

            deltaDentalClaim.DeltaDentalPays = totalParts[i++];
            deltaDentalClaim.PatientPays = totalParts[i++];
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            deltaDentalClaim.ParseException = ex;
        }

        document = deltaDentalClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
