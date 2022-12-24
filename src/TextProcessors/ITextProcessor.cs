using System.Collections.Generic;
using Sc.Pdf.Documents;

namespace Sc.Pdf.TextProcessors;

public interface ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document);
}
