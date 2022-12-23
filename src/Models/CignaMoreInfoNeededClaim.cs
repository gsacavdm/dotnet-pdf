using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Models;

public class CignaMoreInfoNeededClaim : IClaim
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }

    public string FileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Cigna {this.ClaimNumber} {this.DateProcessed.ToDateString()} More Info Needed.pdf";
        }
    }


    public CignaMoreInfoNeededClaim(IEnumerable<string> text)
    {
        this.ClaimNumber = text.ExtractFieldByStartsWith("Claim Number:");
        this.ProviderName = text.ExtractFieldByStartsWith("Provider:");
        this.DateOfService = text.ExtractFieldByStartsWith("Date of Service:").Split(" - ")[0].ParseDate();
        this.DateProcessed = text.ExtractFieldPreviousLineByStartsWith("Subscriber:").ParseDate();
    }

    public bool IsValid => this.ClaimNumber != null;

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateProcessed:yyyy/MM/dd}\",");
        Console.Write($"\"{this.ClaimNumber}\"");

        Console.WriteLine();
    }

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("DateOfService,");
        Console.Write("DateProcessed,");
        Console.Write("ClaimNumber");

        Console.WriteLine();
    }

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date processed: {this.DateProcessed:yyyy/MM/dd}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");

        Console.WriteLine("==============");

        Console.WriteLine($"Final provider name: {this.FileName}");
    }
}
