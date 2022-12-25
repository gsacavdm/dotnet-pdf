using System;
using System.Collections.Generic;
using Sc.PdfProcessor.Extensions;

namespace Sc.PdfProcessor.Documents;

public class CignaMoreInfoNeededClaim : Document, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }
    public string MemberName { get; set; }

    public override string StandardFileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Cigna {this.ClaimNumber} {this.DateProcessed.ToDateString()} More Info Needed.pdf";
        }
    }

    public bool IsValid => !string.IsNullOrEmpty(this.ClaimNumber);

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.MemberName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateProcessed:yyyy/MM/dd}\",");
        Console.Write($"\"{this.ClaimNumber}\"");

        Console.WriteLine();
    }

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("MemberName,");
        Console.Write("DateOfService,");
        Console.Write("DateProcessed,");
        Console.Write("ClaimNumber");

        Console.WriteLine();
    }

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Member name: {this.MemberName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date processed: {this.DateProcessed:yyyy/MM/dd}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");

        Console.WriteLine("==============");

        Console.WriteLine($"Standard file name: {this.StandardFileName}");
    }
}
