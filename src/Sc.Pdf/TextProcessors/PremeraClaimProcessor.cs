using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.TextProcessors;

public class PremeraClaimProcessor : ITextProcessor
{

    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var premeraClaim = new PremeraClaim
        {
            SourceText = text
        };

        try
        {

            premeraClaim.MemberName = text.ExtractFieldByStartsWith("for").Split(",")[0]?.ToTitleCase();

            var servicesProvidedLine = text.ExtractFieldByStartsWith("For services provided by");
            var servicesProvidedParts = servicesProvidedLine.Split(" on ");
            premeraClaim.ProviderName = servicesProvidedParts[0].ToTitleCase();

            // This will be inaccurate some time as there are a few EoBs that
            // cover services across multiple dates. This line will still exist
            // and have a single date (the last one in the grid with all the services listed)
            // Dealing with these kinds of EoBs warrants a bigger refactor...
            premeraClaim.DateOfService = servicesProvidedParts.Length > 1 ? servicesProvidedParts[1].ParseDate() : null;

            premeraClaim.DateProcessed = text.ExtractFieldNextLineByEquals("Customer Service").ParseDate();
            premeraClaim.ClaimNumber = text.ExtractFieldNextLineByStartsWith("Claim #").Split(" / ")[0];
            premeraClaim.AmountBilled = text.ExtractFieldNextLineByEquals("Amount Billed").ParseDouble(); ;
            premeraClaim.NetworkDiscount = text.ExtractFieldNextLineByEquals("Premera Network Discount if applicable").ParseDouble();
            premeraClaim.PaidByHealthPlan = text.ExtractFieldNextLineByEquals("Amount Paid By Your Health Plan").ParseDouble();
            premeraClaim.FromAnotherSource = text.ExtractFieldNextLineByEquals("Amount From Another Source if applicable").ParseDouble();
            premeraClaim.YourResponsibility = text.ExtractFieldNextLineByEquals("Your Total Responsibility").ParseDouble();

            var totalLine = text.ExtractFieldByStartsWith("Totals");
            var totalParts = totalLine.Split(" ");
            if (totalParts.Length == 9)
            {
                premeraClaim.TotalPlanDiscountsAndPayments = totalParts[4].ParseDouble();
                premeraClaim.Deductible = totalParts[5].ParseDouble();
                premeraClaim.Coinsurance = totalParts[6].ParseDouble();
                premeraClaim.NotCovered = totalParts[7].ParseDouble();
            }

            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            premeraClaim.ParseException = ex;
        }

        document = premeraClaim;
        return parsedSuccessfully && document.IsValid;
    }
}
