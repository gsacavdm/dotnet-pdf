using System.Collections.Generic;
using Sc.Pdf.Extensions;
using Xunit;

namespace Sc.Pdf.Test.StringExtensionTests;

public class ExtractFieldNextLineByStartsWithTests
{

    [Fact]
    public void ExtractFieldNextLineByStartsWithHandleNoMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldNextLineByStartsWith("FOO"));
    }
    [Fact]
    public void ExtractFieldNextLineByStartsWithReturnFirstMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Equal("Some A", text.ExtractFieldNextLineByStartsWith("A100:"));
    }
    [Fact]
    public void ExtractFieldNextLineByStartsWithReturnSecondMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Equal("Some A2", text.ExtractFieldNextLineByStartsWith("A100:", 2));
    }
    [Fact]
    public void ExtractFieldNextLineByStartsWithHandleNoNthMatch()
    {
        var text = new List<string> {
            "A100: Ignore Me",
            "Some A",
            "B200: Ignore Me",
            "Some B",
            "A100: Ignore Me",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldNextLineByStartsWith("A100:", 3));
    }
}
