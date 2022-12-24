using System;
using System.Collections.Generic;

namespace Sc.Pdf.Documents;

public interface IDocument
{
    public IEnumerable<string> SourceText { get; }
    public string SourceFileName { get; set; }
    public string StandardFileName { get; }
    public Exception ParseException { get; }
    public bool IsValid { get; }

    public void WriteLine();
    public void WriteCsv();
    public void WriteCsvHeader();
}
