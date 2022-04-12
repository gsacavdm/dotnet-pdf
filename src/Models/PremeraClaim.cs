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
    public double? DiscountedRate { get; set; }
    public double? AmountPaid { get; set; }
    public double? AmountYouOwe { get; set; }
    public string FileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Premera {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
        }
    }

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService}");
        Console.WriteLine($"Date processed: {this.DateProcessed}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");
        Console.WriteLine($"Amount billed: {this.AmountBilled}");
        Console.WriteLine($"Discounted rate: {this.DiscountedRate}");
        Console.WriteLine($"Amount paid: {this.AmountPaid}");
        Console.WriteLine($"Amount you owe: {this.AmountYouOwe}");

        Console.WriteLine("==============");

        Console.WriteLine($"Final provider name: {this.FileName}");
    }
}