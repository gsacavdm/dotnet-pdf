using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Models;

public class CignaClaim : IClaim
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }

    public double? AmountBilled { get; set; }
    public double? Discount { get; set; }
    public double? AmountPaid { get; set; }
    public double? AmountYouOwe { get; set; }
    public string FileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Cigna {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
        }
    }


    public CignaClaim(IEnumerable<string> text)
    {
        var claimNumber = text.ExtractFieldNextLineByEquals("Claim # / ID").Split(" ")[0];
        var providerName = text.ExtractFieldByStartsWith("for services provided by");

        var dateOfServiceLine = text.ExtractFieldNextLineByEquals("Service date");
        if (string.IsNullOrEmpty(dateOfServiceLine))
        {
            dateOfServiceLine = text.ExtractFieldNextLineByEquals("Service dates");
        }
        var dateOfService = dateOfServiceLine.Split(" - ")[0].ParseDate();

        var amountBilled = text.ExtractFieldNextLineByEquals("Amount Billed").ParseDouble();
        var discount = text.ExtractFieldNextLineByEquals("Discount").ParseDouble();
        var amountPaid = text.ExtractFieldNextLineByEquals("paid").ParseDouble();
        var amountYouOwe = text.ExtractFieldNextLineByEquals("What I Owe").ParseDouble();

        var dateProcessedLine = text.ExtractFieldByStartsWith("Cigna received this claim on");
        var dateProcessed = dateProcessedLine.Split("processed it on ")[1].Replace(".", "").ParseDate();

        this.ClaimNumber = claimNumber;
        this.ProviderName = providerName;
        this.DateOfService = dateOfService;
        this.AmountBilled = amountBilled;
        this.Discount = discount;
        this.AmountPaid = amountPaid;
        this.AmountYouOwe = amountYouOwe;
        this.DateProcessed = dateProcessed;
    }

    public bool IsValid => this.AmountBilled != null;

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

        Console.WriteLine($"Final provider name: {this.FileName}");
    }
}
