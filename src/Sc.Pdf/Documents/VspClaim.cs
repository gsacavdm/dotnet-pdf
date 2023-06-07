using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Documents;

public class VspClaim : DocumentBase, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateOfNotice { get; set; }
    public string ProviderName { get; set; }
    public string MemberName { get; set; }

    public double? AmountBilled { get; set; }
    public double? AmountPaid { get; set; }

    protected override string GetStandardFileName()
    {
        var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
        return $"{this.DateOfService.ToDateString()} {providerName} Claim Vsp {this.ClaimNumber} {this.DateOfNotice.ToDateString()}.pdf";
    }

    protected override bool GetIsValid() => !string.IsNullOrEmpty(this.ProviderName) && !string.IsNullOrEmpty(this.ClaimNumber);
}
