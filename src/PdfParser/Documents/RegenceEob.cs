using System;
using PdfParser.Extensions;

namespace PdfParser.Documents;

public class RegenceEob : DocumentBase, IDocument
{

    public DateTime? PrintDate { get; set; }
    protected override string GetStandardFileName() => $"{this.PrintDate.ToDateString()}.pdf";

    protected override bool GetIsValid() => this.PrintDate != null;
}
