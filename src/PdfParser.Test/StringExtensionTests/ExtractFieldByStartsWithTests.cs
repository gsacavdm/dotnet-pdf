using System.Collections.Generic;
using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ExtractFieldByStartsWithTests
{
    [Fact]
    public void ExtractFieldByStartsWithHandleNoMatch()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B"
        };

        Assert.Empty(text.ExtractFieldByStartsWith("FOO"));
    }
    [Fact]
    public void ExtractFieldByStartsWithReturnFirstMatch()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B",
            "A100: Some A2",
            "B200: Some B2"
        };

        Assert.Equal("Some A", text.ExtractFieldByStartsWith("A100:"));
    }
    [Fact]
    public void ExtractFieldByStartsWithReturnSecondMatch()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B",
            "A100: Some A2",
            "B200: Some B2"
        };

        Assert.Equal("Some A2", text.ExtractFieldByStartsWith("A100:", 2));
    }
    [Fact]
    public void ExtractFieldByStartsWithHandleNoNthMatch()
    {
        var text = new List<string> {
            "A100: Some A",
            "B200: Some B",
            "A100: Some A2",
            "B200: Some B2"
        };

        Assert.Empty(text.ExtractFieldByStartsWith("A100:", 3));
    }
}
