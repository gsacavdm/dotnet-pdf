using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Documents;

public class RegenceClaim : Document, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }

    public string PharmacyName { get; set; }
    public DateTime? DateOfFill { get; set; }
    public string MedicationName { get; set; }
    public string PrescriberName { get; set; }
    public string PrescriptionNumber { get; set; }
    public string NdcNumber { get; set; }

    public double? AmountBilled { get; set; }
    public double? DiscountedRate { get; set; }
    public double? AmountPaid { get; set; }
    public double? AmountYouOwe { get; set; }
    public override string StandardFileName
    {
        get
        {
            var dateOfService = this.DateOfService;
            var dateProcessed = this.DateProcessed;
            var providerName = this.ProviderName;
            var claimNumber = this.ClaimNumber;

            var pharmacyName = string.Empty;
            var medicationName = string.Empty;

            if (!string.IsNullOrEmpty(this.PharmacyName))
            {
                dateOfService = this.DateOfFill;
                dateProcessed = this.DateOfFill;
                providerName = this.PrescriberName;

                pharmacyName = this.PharmacyName.Split("#")[0].Trim();
                medicationName = this.MedicationName.Split(" ")[0];
                claimNumber = $"{this.PrescriptionNumber}-{this.DateOfFill.ToShortDateString()}-{this.NdcNumber}";
            }

            providerName = InsuranceExtensions.MapProvider(providerName);

            if (!string.IsNullOrEmpty(this.PharmacyName))
            {
                providerName = $"{providerName} {pharmacyName} {medicationName}";
            }

            return string.IsNullOrEmpty(providerName) ? string.Empty : $"{dateOfService.ToDateString()} {providerName} Claim Regence {claimNumber} {dateProcessed.ToDateString()}.pdf";
        }
    }

    public bool IsValid => this.AmountYouOwe != null;

    public void WriteLine()
    {
        Console.WriteLine("==============");

        Console.WriteLine($"Provider name: {this.ProviderName}");
        Console.WriteLine($"Date of service: {this.DateOfService:yyyy/MM/dd}");
        Console.WriteLine($"Date processed: {this.DateProcessed:yyyy/MM/dd}");
        Console.WriteLine($"Pharmacy name: {this.PharmacyName}");
        Console.WriteLine($"Date of fill: {this.DateOfFill}");
        Console.WriteLine($"Medication name: {this.MedicationName}");
        Console.WriteLine($"Prescriber name: {this.PrescriberName}");
        Console.WriteLine($"Prescription number: {this.PrescriptionNumber}");
        Console.WriteLine($"Ndc number: {this.NdcNumber}");
        Console.WriteLine($"Claim number: {this.ClaimNumber}");
        Console.WriteLine($"Amount billed: {this.AmountBilled}");
        Console.WriteLine($"Discounted rate: {this.DiscountedRate}");
        Console.WriteLine($"Amount paid: {this.AmountPaid}");
        Console.WriteLine($"Amount you owe: {this.AmountYouOwe}");

        Console.WriteLine("==============");

        Console.WriteLine($"Standard file name: {this.StandardFileName}");
    }

    public void WriteCsv()
    {
        Console.Write($"\"{this.ProviderName}\",");
        Console.Write($"\"{this.DateOfService:yyyy/MM/dd}\",");
        Console.Write($"\"{this.DateProcessed:yyyy/MM/dd}\",");
        Console.Write($"\"{this.PharmacyName}\",");
        Console.Write($"\"{this.DateOfFill:yyyy/MM/dd}\",");
        Console.Write($"\"{this.MedicationName}\",");
        Console.Write($"\"{this.PrescriberName}\",");
        Console.Write($"\"{this.PrescriptionNumber}\",");
        Console.Write($"\"{this.NdcNumber}\",");
        Console.Write($"\"{this.ClaimNumber}\",");
        Console.Write($"\"{this.AmountBilled}\",");
        Console.Write($"\"{this.DiscountedRate}\",");
        Console.Write($"\"{this.AmountPaid}\",");
        Console.Write($"\"{this.AmountYouOwe}\"");

        Console.WriteLine();
    }

    public void WriteCsvHeader()
    {
        Console.Write("ProviderName,");
        Console.Write("DateOfService,");
        Console.Write("DateProcessed,");
        Console.Write("PharmacyName,");
        Console.Write("DateOfFill,");
        Console.Write("MedicationName,");
        Console.Write("PrescriberName,");
        Console.Write("PrescriptionNumber,");
        Console.Write("NdcNumber,");
        Console.Write("ClaimNumber,");
        Console.Write("AmountBilled,");
        Console.Write("DiscountedRate,");
        Console.Write("AmountPaid,");
        Console.Write("AmountYouOwe");

        Console.WriteLine();
    }
}
