using Sc.Pdf.Documents;
using Xunit;

namespace Sc.Pdf.Test;

public class DocumentBaseTest
{
    public class MockDocumentBase : DocumentBase
    {
        public string? PropA { get; set; }
        public string? PropB { get; set; }
        [CsvIgnore]
        public string? PropIgnore { get; set; }
        public override string StandardFileName => "MyStandardFileNames";
    }

    [Fact]
    public void DocumentBaseWriteCsv()
    {
        var documentBase = new MockDocumentBase();
        documentBase.WriteCsvHeader();
        documentBase.WriteCsv();
    }
}
