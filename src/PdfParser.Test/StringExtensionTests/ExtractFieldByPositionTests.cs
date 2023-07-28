using System.Collections.Generic;
using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ExtractFieldByPositionTests
{
    [Fact]
    public void ExtractFieldByPositionHandleInvalidPosition()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B"
        };

        Assert.Empty(text.ExtractFieldByPosition(10));
    }
    [Fact]
    public void ExtractFieldByPositionHandleValidPosition()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B"
        };

        Assert.Equal("A100: Some A", text.ExtractFieldByPosition(0));
    }
}
