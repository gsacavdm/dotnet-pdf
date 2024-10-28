using System;
using PdfParser.Extensions;

namespace PdfParser.Documents;

public class DeltaDentalClaim : DocumentBase, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }
    public string MemberName { get; set; }

    public double? SubmittedFee { get; set; }
    public double? AcceptedFee { get; set; }
    public double? MaximumContractAllowance { get; set; }
    public double? AmountAppliedToDeductible { get; set; }
    public double? PaidByAnotherPlan { get; set; }
    public double? ContactBenefitLevel { get; set; }
    public double? DeltaDentalPays { get; set; }
    public double? PatientPays { get; set; }

    protected override string GetStandardFileName()
    {
        var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
        return $"{this.DateOfService.ToDateString()} {providerName} Claim Delta {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
    }

    protected override bool GetIsValid() => !string.IsNullOrEmpty(this.ProviderName) && !string.IsNullOrEmpty(this.ClaimNumber);
}
