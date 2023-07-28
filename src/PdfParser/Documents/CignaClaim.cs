using System;
using PdfParser.Extensions;

namespace PdfParser.Documents;

public class CignaClaim : DocumentBase, IDocument
{
    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string ProviderName { get; set; }
    public string MemberName { get; set; }

    public double? AmountBilled { get; set; }
    public double? Discount { get; set; }
    public double? AmountNotCovered { get; set; }
    public double? AllowedAmount { get; set; }
    public double? Copay { get; set; }
    public double? Deductible { get; set; }
    public double? AmountPaid { get; set; }
    public double? Coinsurance { get; set; }
    public double? AmountYouOwe { get; set; }

    public CignaClaimType ClaimType { get; set; }

    public enum CignaClaimType
    {
        EoB,
        Web,
        MoreInfoNeeded
    }

    protected override string GetStandardFileName()
    {
        var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
        var qualifier = this.ClaimType == CignaClaimType.Web ? " Web" :
            this.ClaimType == CignaClaimType.MoreInfoNeeded ? " More Info Needed" :
            "";
        return $"{this.DateOfService.ToDateString()} {providerName} Claim Cigna {this.ClaimNumber} {this.DateProcessed.ToDateString()}{qualifier}.pdf";
    }

    protected override bool GetIsValid() =>
        !string.IsNullOrEmpty(this.ProviderName)
        && this.DateOfService != null
        && (
            this.AmountBilled != null
            || this.ClaimType == CignaClaimType.MoreInfoNeeded
    );
}
