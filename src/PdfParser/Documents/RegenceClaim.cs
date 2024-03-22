using System;
using PdfParser.Extensions;

namespace PdfParser.Documents;

public class RegenceClaim : DocumentBase, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }
    public string MemberName { get; set; }

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
    protected override string GetStandardFileName()
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

    protected override bool GetIsValid() => this.AmountYouOwe != null;
}
