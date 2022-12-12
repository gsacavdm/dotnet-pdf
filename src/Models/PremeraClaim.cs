using System;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Models;

public class PremeraClaim
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

    public bool IsInvalid => this.ProviderName == null;

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService}");
        Console.WriteLine($"Date processed: {this.DateProcessed}");
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
        Console.Write("\"" + this.ProviderName + "\",");
        Console.Write("\"" + this.DateOfService + "\",");
        Console.Write("\"" + this.DateProcessed + "\",");
        Console.Write("\"" + this.ClaimNumber + "\",");
        Console.Write("\"" + this.AmountBilled + "\",");
        Console.Write("\"" + this.NetworkDiscount + "\",");
        Console.Write("\"" + this.PaidByHealthPlan + "\",");
        Console.Write("\"" + this.FromAnotherSource + "\",");
        Console.Write("\"" + this.TotalPlanDiscountsAndPayments + "\",");
        Console.Write("\"" + this.Deductible + "\",");
        Console.Write("\"" + this.Coinsurance + "\",");
        Console.Write("\"" + this.NotCovered + "\",");
        Console.Write("\"" + this.YourResponsibility + "\"");

        Console.WriteLine();
    }
}
