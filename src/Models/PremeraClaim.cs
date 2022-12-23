using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Models;

public class PremeraClaim : IClaim
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }

    public double? AmountBilled { get; set; }
    public double? NetworkDiscount { get; set; }
    public double? PaidByHealthPlan { get; set; }
    public double? FromAnotherSource { get; set; }
    public double? TotalPlanDiscountsAndPayments { get; set; }
    public double? Deductible { get; set; }
    public double? Coinsurance { get; set; }
    public double? NotCovered { get; set; }
    public double? YourResponsibility { get; set; }

    public string FileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Premera {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
        }
    }

    public PremeraClaim(IEnumerable<string> text)
    {
        foreach (var line in text)
        {
            if (line.StartsWith("For services provided by", false, CultureInfo.InvariantCulture))
            {
                var tmp = line.Replace("For services provided by ", "");
                var parts = tmp.Split(" on ");

                if (parts.Length == 2)
                {
                    this.ProviderName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[0].ToLower(CultureInfo.InvariantCulture));

                    // This will be inaccurate some time as there are a few EoBs that
                    // cover services across multiple dates. This line will still exist
                    // and have a single date (the last one in the grid with all the services listed)
                    // Dealing with these kinds of EoBs warrants a bigger refactor...
                    this.DateOfService = DateTime.TryParse(parts[1], out var date) ? date : null;
                }
            }
            else if (Regex.IsMatch(line, "[a-zA-Z]+ [0-9]{2}, [0-9]{4}") && this.DateProcessed == null)
            {
                this.DateProcessed = DateTime.TryParse(line, out var date) ? date : null;
            }
            else if (Regex.IsMatch(line, "Claim #"))
            {
                var parts = line.Split("# ");
                this.ClaimNumber = parts[1].Split(",")[0];
            }
            else if (line.StartsWith("Totals", false, CultureInfo.InvariantCulture))
            {
                // Indexes would need to be +1 from the other rows that aren't the total
                // as those include a column on position 1 with the date of service.
                var amounts = line.Split(" ");
                this.AmountBilled = amounts[1].ParseDouble();
                this.NetworkDiscount = amounts[2].ParseDouble();
                this.PaidByHealthPlan = amounts[3].ParseDouble();
                this.FromAnotherSource = amounts[4].ParseDouble();
                this.TotalPlanDiscountsAndPayments = amounts[5].ParseDouble();
                this.Deductible = amounts[6].ParseDouble();
                this.Coinsurance = amounts[7].ParseDouble();
                this.NotCovered = amounts[8].ParseDouble();
                this.YourResponsibility = amounts[9].ParseDouble();
            }
        }
    }

    public bool IsValid => this.ProviderName != null;

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date processed: {this.DateProcessed:yyyy/MM/dd}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");
        Console.WriteLine($"Amount billed: {this.AmountBilled}");
        Console.WriteLine($"Network Discount: {this.NetworkDiscount}");
        Console.WriteLine($"Paid By Health Plan: {this.PaidByHealthPlan}");
        Console.WriteLine($"From Another Source: {this.FromAnotherSource}");
        Console.WriteLine($"Total Plan Discounts And Payments: {this.TotalPlanDiscountsAndPayments}");
        Console.WriteLine($"Deductible: {this.Deductible}");
        Console.WriteLine($"Coinsurance: {this.Coinsurance}");
        Console.WriteLine($"NotCovered: {this.NotCovered}");
        Console.WriteLine($"Your Responsibility: {this.YourResponsibility}");

        Console.WriteLine("==============");

        Console.WriteLine($"Final provider name: {this.FileName}");
    }

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateProcessed:yyyy/MM/dd}\",");
        Console.Write($"\"{this.ClaimNumber}\",");
        Console.Write($"\"{this.AmountBilled}\",");
        Console.Write($"\"{this.NetworkDiscount}\",");
        Console.Write($"\"{this.PaidByHealthPlan}\",");
        Console.Write($"\"{this.FromAnotherSource}\",");
        Console.Write($"\"{this.TotalPlanDiscountsAndPayments}\",");
        Console.Write($"\"{this.Deductible}\",");
        Console.Write($"\"{this.Coinsurance}\",");
        Console.Write($"\"{this.NotCovered}\",");
        Console.Write($"\"{this.YourResponsibility}\"");

        Console.WriteLine();
    }

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("DateOfService,");
        Console.Write("DateProcessed,");
        Console.Write("ClaimNumber,");
        Console.Write("AmountBilled,");
        Console.Write("NetworkDiscount,");
        Console.Write("PaidByHealthPlan,");
        Console.Write("FromAnotherSource,");
        Console.Write("TotalPlanDiscountsAndPayments,");
        Console.Write("Deductible,");
        Console.Write("Coinsurance,");
        Console.Write("NotCovered,");
        Console.Write("YourResponsibility");

        Console.WriteLine();
    }
}
