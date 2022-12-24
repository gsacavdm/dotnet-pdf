using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;
using Sc.Pdf.TextProcessors;

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
            foreach (var line in text)
            {
                if (line.StartsWith("For services provided by", false, CultureInfo.InvariantCulture))
                {
                    var tmp = line.Replace("For services provided by ", "");
                    var parts = tmp.Split(" on ");

                    if (parts.Length == 2)
                    {
                        premeraClaim.ProviderName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[0].ToLower(CultureInfo.InvariantCulture));

                        // This will be inaccurate some time as there are a few EoBs that
                        // cover services across multiple dates. This line will still exist
                        // and have a single date (the last one in the grid with all the services listed)
                        // Dealing with these kinds of EoBs warrants a bigger refactor...
                        premeraClaim.DateOfService = DateTime.TryParse(parts[1], out var date) ? date : null;
                    }
                }
                else if (Regex.IsMatch(line, "[a-zA-Z]+ [0-9]{2}, [0-9]{4}") && premeraClaim.DateProcessed == null)
                {
                    premeraClaim.DateProcessed = DateTime.TryParse(line, out var date) ? date : null;
                }
                else if (Regex.IsMatch(line, "Claim #"))
                {
                    var parts = line.Split("# ");
                    premeraClaim.ClaimNumber = parts[1].Split(",")[0];
                }
                else if (line.StartsWith("Totals", false, CultureInfo.InvariantCulture))
                {
                    // Indexes would need to be +1 from the other rows that aren't the total
                    // as those include a column on position 1 with the date of service.
                    var amounts = line.Split(" ");
                    premeraClaim.AmountBilled = amounts[1].ParseDouble();
                    premeraClaim.NetworkDiscount = amounts[2].ParseDouble();
                    premeraClaim.PaidByHealthPlan = amounts[3].ParseDouble();
                    premeraClaim.FromAnotherSource = amounts[4].ParseDouble();
                    premeraClaim.TotalPlanDiscountsAndPayments = amounts[5].ParseDouble();
                    premeraClaim.Deductible = amounts[6].ParseDouble();
                    premeraClaim.Coinsurance = amounts[7].ParseDouble();
                    premeraClaim.NotCovered = amounts[8].ParseDouble();
                    premeraClaim.YourResponsibility = amounts[9].ParseDouble();
                }
            }
            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            premeraClaim.ParseException = ex;
        }

        document = premeraClaim;
        return parsedSuccessfully;
    }
}
