using System.Collections.Generic;
using PdfParser.Documents;

namespace PdfParser.TextProcessors;

public interface ITextProcessor
{
    public bool TryParse(IEnumerable<string> text, out IDocument document);
}
