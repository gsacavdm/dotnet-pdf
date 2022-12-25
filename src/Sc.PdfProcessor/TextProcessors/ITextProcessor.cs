using System.Collections.Generic;
using Sc.PdfProcessor.Documents;

namespace Sc.PdfProcessor.TextProcessors;

public interface ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document);
}
