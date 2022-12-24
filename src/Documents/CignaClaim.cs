using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Documents;

public class CignaClaim : Document, IDocument
{
    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }

    public double? AmountBilled { get; set; }
    public double? Discount { get; set; }
    public double? AmountPaid { get; set; }
    public double? AmountYouOwe { get; set; }

    public override string StandardFileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Cigna {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
        }
    }

    public bool IsValid => !string.IsNullOrEmpty(this.ProviderName) && this.AmountBilled != null;

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("DateOfService,");
        Console.Write("DateProcessed,");
        Console.Write("ClaimNumber,");
        Console.Write("AmountBilled,");
        Console.Write("Discount,");
        Console.Write("AmountPaid,");
        Console.Write("AmountYouOwe");

        Console.WriteLine();
    }

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateProcessed:yyyy/MM/dd}\",");
        Console.Write($"\"{this.ClaimNumber}\",");
        Console.Write($"\"{this.AmountBilled}\",");
        Console.Write($"\"{this.Discount}\",");
        Console.Write($"\"{this.AmountPaid}\",");
        Console.Write($"\"{this.AmountYouOwe}\"");

        Console.WriteLine();
    }

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date processed: {this.DateProcessed:yyyy/MM/dd}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");
        Console.WriteLine($"Amount billed: {this.AmountBilled}");
        Console.WriteLine($"Discount: {this.Discount}");
        Console.WriteLine($"Amount paid: {this.AmountPaid}");
        Console.WriteLine($"Amount you owe: {this.AmountYouOwe}");

        Console.WriteLine("==============");

        Console.WriteLine($"Standard file name: {this.StandardFileName}");
    }
}
