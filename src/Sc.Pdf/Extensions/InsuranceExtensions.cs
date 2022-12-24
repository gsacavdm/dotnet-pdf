using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sc.Pdf.Extensions;

public static class InsuranceExtensions
{

    public static string MapProvider(string providerName)
    {
        if (providerName.Contains(','))
        {
            var nameParts = providerName.Split(",");
            providerName = nameParts[1].Trim() + " " + nameParts[0].Trim();
        }

        providerName = providerName.Replace(",", "");
        providerName = providerName.Replace(".", "");
        providerName = providerName.Replace(" Inc ", " ");

        // Remove middle names (e.g. Jason J Lukas -> remove the J)
        // Remove professional abbreviations (e.g. Md, Po, Pt)
        providerName = string.Join(" ", providerName.Split(" ").Where(s => s.Length > 2));

        providerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(providerName.ToLower(CultureInfo.InvariantCulture));

        var mappedProviders = new Dictionary<string, string>
        {
            {"Jason Lukas", "Physiatrist"},
            {"Labcorp Of America", "Labcorp"},
            {"Mary Anderson", "Therapy"},
            {"Andrew Appelbaum", "Family Doctor"},
            {"Andrew Mark Appelbaum", "Family Doctor"},
            {"Daniel Trautman", "Massage"},
            {"Na No Provider Found", "No Provider"}
            // ... add more ...
        };

        mappedProviders.TryGetValue(providerName, out var mappedProvider);

        return mappedProvider ?? providerName;
    }
}
