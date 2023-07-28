using System;
using PdfParser.Extensions;

namespace PdfParser.Documents;

public class TruistStatement : DocumentBase, IDocument
{
    public DateTime? DueDate { get; set; }
    public DateTime? StatementDate { get; set; }
    public double? PrincipalBalance { get; set; }
    public double? EscrowBalance { get; set; }
    public double? AmountDue { get; set; }
    public double? PrincipalDue { get; set; }
    public double? InterestDue { get; set; }
    public double? EscrowTaxesAndInsurance { get; set; }

    protected override string GetStandardFileName() => $"{this.StatementDate.ToDateString()} Statement.pdf";

    protected override bool GetIsValid() => this.StatementDate != null;
}
