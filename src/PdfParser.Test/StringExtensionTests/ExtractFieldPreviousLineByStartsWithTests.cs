using System.Collections.Generic;
using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ExtractFieldPreviousLineByStartsWithTests
{

    [Fact]
    public void ExtractFieldPreviousLineByStartsWithHandleNoMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldPreviousLineByStartsWith("FOO"));
    }
    [Fact]
    public void ExtractFieldPreviousLineByStartsWithReturnFirstMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Equal("Some A", text.ExtractFieldPreviousLineByStartsWith("B200:"));
    }
    [Fact]
    public void ExtractFieldPreviousLineByStartsWithReturnSecondMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
            "B200: Ignore Me",
        };

        Assert.Equal("Some A2", text.ExtractFieldPreviousLineByStartsWith("B200:", 2));
    }
    [Fact]
    public void ExtractFieldPreviousLineByStartsWithHandleNoNthMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
            "B200: Ignore Me",
        };

        Assert.Empty(text.ExtractFieldPreviousLineByStartsWith("B200:", 3));
    }
}
