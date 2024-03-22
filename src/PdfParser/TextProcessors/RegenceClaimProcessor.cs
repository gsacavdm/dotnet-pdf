using System;
using System.Collections.Generic;
using PdfParser.Documents;
using PdfParser.Extensions;

namespace PdfParser.TextProcessors;

public class RegenceClaimProcessor : ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var regenceClaim = new RegenceClaim
        {
            SourceText = text
        };

        try
        {
            regenceClaim.ProviderName = text.ExtractFieldByStartsWith("Provider name");
            regenceClaim.MemberName = text.ExtractFieldByStartsWith("Member name");
            regenceClaim.DateOfService = text.ExtractFieldByStartsWith("Date of service").ParseDate();
            regenceClaim.DateProcessed = text.ExtractFieldByStartsWith("Date processed").ParseDate();
            regenceClaim.PharmacyName = text.ExtractFieldByStartsWith("Pharmacy name");
            regenceClaim.NdcNumber = text.ExtractFieldByStartsWith("NDC number");
            regenceClaim.PrescriptionNumber = text.ExtractFieldByStartsWith("Prescription number");
            regenceClaim.DateOfFill = text.ExtractFieldByStartsWith("Date of fill").ParseDate();
            regenceClaim.MedicationName = text.ExtractFieldByStartsWith("Medication name").Replace("/", "-").ToTitleCase();
            regenceClaim.PrescriberName = text.ExtractFieldByStartsWith("Prescriber name");
            regenceClaim.ClaimNumber = text.ExtractFieldByStartsWith("Claim number");
            regenceClaim.AmountBilled = text.ExtractFieldByStartsWith("Amount billed").ParseDouble();
            regenceClaim.DiscountedRate = text.ExtractFieldByStartsWith("Your discounted rate").ParseDouble()
                ?? text.ExtractFieldByStartsWith("Y our discounted rate").ParseDouble(); // Some PDFs are parsed like this for some reason...
            regenceClaim.AmountPaid = text.ExtractFieldByStartsWith("Amount we paid").ParseDouble();
            regenceClaim.AmountYouOwe = text.ExtractFieldByStartsWith("Amount you owe").ParseDouble()
                ?? text.ExtractFieldByStartsWith("Amount you may owe").ParseDouble();
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            regenceClaim.ParseException = ex;
        }

        document = regenceClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
