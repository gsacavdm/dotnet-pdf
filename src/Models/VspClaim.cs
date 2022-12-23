using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Models;

public class VspClaim : IClaim
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateOfNotice { get; set; }
    public string ProviderName { get; set; }

    public double? AmountBilled { get; set; }
    public double? AmountPaid { get; set; }

    public string FileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Vsp {this.ClaimNumber} {this.DateOfNotice.ToDateString()}.pdf";
        }
    }

    public VspClaim(IEnumerable<string> text)
    {
        this.ClaimNumber = text.ExtractFieldByStartsWith("Claim #:").Split(" ")[0];
        this.DateOfService = text.ExtractFieldByStartsWith("Date of Service:").ParseDate();
        this.DateOfNotice = text.ExtractFieldByStartsWith("Date of Notice:").ParseDate();
        this.ProviderName = text.ExtractFieldByStartsWith("Provider:");
        this.AmountBilled = text.ExtractFieldNextLineByEquals("Billed:").ParseDouble();
        this.AmountPaid = text.ExtractFieldNextLineByEquals("Paid:").ParseDouble();
    }

    public bool IsValid => this.ProviderName != null;

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date of notice: {this.DateOfNotice:yyyy/MM/dd}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");
        Console.WriteLine($"Amount billed: {this.AmountBilled}");
        Console.WriteLine($"Amount paid: {this.AmountPaid}");

        Console.WriteLine("==============");

        Console.WriteLine($"Final provider name: {this.FileName}");
    }

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateOfNotice:yyyy/MM/dd}\",");
        Console.Write($"\"{this.ClaimNumber}\",");
        Console.Write($"\"{this.AmountBilled}\",");
        Console.Write($"\"{this.AmountPaid}\",");

        Console.WriteLine();
    }

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("DateOfService,");
        Console.Write("DateOfNotice,");
        Console.Write("ClaimNumber,");
        Console.Write("AmountBilled,");
        Console.Write("AmountPaid,");

        Console.WriteLine();
    }
}
