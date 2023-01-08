using System;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.Documents;

public class TruistStatement : DocumentBase, IDocument
{
    public DateTime? DueDate { get; set; }
    public DateTime? StatementDate { get; set; }

    //ToDo
    //public double? PrincipalBalance { get; set; }
    public double? EscrowBalance { get; set; }
    public double? AmountDue { get; set; }

    //ToDo
    //public double? PrincipalDue { get; set; }
    //public double? InterestDue { get; set; }
    public double? EscrowTaxesAndInsurance { get; set; }

    protected override string GetStandardFileName() => $"{this.StatementDate.ToDateString()} Statement.pdf";

    protected override bool GetIsValid() => this.StatementDate != null;
}
