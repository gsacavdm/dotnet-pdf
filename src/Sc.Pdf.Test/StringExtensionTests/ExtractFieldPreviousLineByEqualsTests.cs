using System.Collections.Generic;
using Sc.Pdf.Extensions;
using Xunit;

namespace Sc.Pdf.Test.StringExtensionTests;

public class ExtractFieldPreviousLineByEqualsTests
{

    [Fact]
    public void ExtractFieldPreviousLineByEqualsHandleNoMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldPreviousLineByEquals("FOO"));
    }
    [Fact]
    public void ExtractFieldPreviousLineByEqualsReturnFirstMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Equal("Some A", text.ExtractFieldPreviousLineByEquals("B200:"));
    }
    [Fact]
    public void ExtractFieldPreviousLineByEqualsReturnSecondMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
            "B200:",
        };

        Assert.Equal("Some A2", text.ExtractFieldPreviousLineByEquals("B200:", 2));
    }
    [Fact]
    public void ExtractFieldPreviousLineByEqualsHandleNoNthMatch()
    {
        var text = new List<string> {
            "A100:",
            "Some A",
            "B200:",
            "Some B",
            "A100:",
            "Some A2",
        };

        Assert.Empty(text.ExtractFieldPreviousLineByEquals("B200:", 3));
    }
}
