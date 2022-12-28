using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Documents;

public class PremeraClaim : DocumentBase, IDocument
{

    public string ClaimNumber { get; set; }
    public DateTime? DateOfService { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string MemberName { get; set; }
    public string ProviderName { get; set; }

    public double? AmountBilled { get; set; }
    public double? NetworkDiscount { get; set; }
    public double? PaidByHealthPlan { get; set; }
    public double? FromAnotherSource { get; set; }
    public double? TotalPlanDiscountsAndPayments { get; set; }
    public double? Deductible { get; set; }
    public double? Coinsurance { get; set; }
    public double? NotCovered { get; set; }
    public double? YourResponsibility { get; set; }

    protected override string GetStandardFileName()
    {
        var providerName = InsuranceExtensions.MapProvider(this.ProviderName);
        return $"{this.DateOfService.ToDateString()} {providerName} Claim Premera {this.ClaimNumber} {this.DateProcessed.ToDateString()}.pdf";
    }

    protected override bool GetIsValid() => !string.IsNullOrEmpty(this.ProviderName);
}
