using System.Collections.Generic;
using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ExtractFieldNextLineByEqualsTests
{

    [Fact]
    public void ExtractFieldNextLineByEqualsHandleNoMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldNextLineByEquals("FOO"));
    }
    [Fact]
    public void ExtractFieldNextLineByEqualsReturnFirstMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Equal("Some A", text.ExtractFieldNextLineByEquals("A100:"));
    }
    [Fact]
    public void ExtractFieldNextLineByEqualsReturnSecondMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Equal("Some A2", text.ExtractFieldNextLineByEquals("A100:", 2));
    }
    [Fact]
    public void ExtractFieldNextLineByEqualsHandleNoNthMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldNextLineByEquals("A100:", 3));
    }
}
