using System.Collections.Generic;
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
        providerName = string.Join(" ", providerName.Split(" ").Where(s => s.Length > 1));

        var mappedProviders = new Dictionary<string, string>
        {
            {"Jason Lukas", "Physiatrist"},
            {"Labcorp Of America", "Labcorp"},
            {"Mary Anderson", "Therapy"},
            {"Andrew Appelbaum", "Family Doctor"},
            {"Daniel Trautman", "Massage"}
            // ... add more ...
        };

        mappedProviders.TryGetValue(providerName, out var mappedProvider);

        return mappedProvider ?? providerName;
    }
}
