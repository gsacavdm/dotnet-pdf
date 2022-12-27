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

    public override string StandardFileName
    {
        get
        {
            var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
            return $"{this.DateOfService.ToDateString()} {providerName} Claim Vsp {this.ClaimNumber} {this.DateOfNotice.ToDateString()}.pdf";
        }
    }

    public bool IsValid => !string.IsNullOrEmpty(this.ProviderName);
}
