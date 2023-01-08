using System.Collections.Generic;
using Sc.Pdf.Documents;
using Xunit;

namespace Sc.Pdf.Test;

public class DocumentBaseTest
{
    private class MockDocumentBase : DocumentBase
    {
        public string? PropA { get; set; }
        public string? PropB { get; set; }
        [CsvIgnore]
        public string? PropIgnore { get; set; }
        protected override string GetStandardFileName() => "MyStandardFileNames";
        protected override bool GetIsValid() => true;
        public MockDocumentBase(IWriter mockWriter) : base(mockWriter)
        {
        }
    }

    private class MockWriter : IWriter
    {
        public List<string> FullOutput = new();
        public void WriteLine(string output) => this.FullOutput.Add(output);
    }

    [Fact]
    public void DocumentBaseWriteCsv()
    {
        var mockWriter = new MockWriter();
        var documentBase = new MockDocumentBase(mockWriter);
        documentBase.WriteCsvHeader();
        documentBase.WriteCsv();

        Assert.NotNull(mockWriter.FullOutput);
        Assert.Equal(2, mockWriter.FullOutput.Count);
        Assert.Equal("PropA,PropB", mockWriter.FullOutput[0]);
        Assert.Equal("\"\",\"\"", mockWriter.FullOutput[1]);
    }
}
