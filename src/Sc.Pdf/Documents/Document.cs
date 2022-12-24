using System;
using System.Collections.Generic;

namespace Sc.Pdf.Documents;

public abstract class Document
{
    public IEnumerable<string> SourceText { get; set; }
    public string SourceFileName { get; set; }
    public Exception ParseException { get; set; }
    public abstract string StandardFileName { get; }
}
