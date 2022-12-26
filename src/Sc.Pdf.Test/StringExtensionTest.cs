using System;
using System.Collections.Generic;
using Sc.Pdf.Extensions;
using Xunit;

namespace Sc.Pdf.Test;

public class StringExtensionTest
{
    [Fact]
    public void ExtractFieldByRegexHandleNoMatch()
    {
        var text = new List<string> {
            "A100",
            "B200"
        };

        Assert.Empty(text.ExtractFieldByRegex("My name is: (?<value>[a-zA-Z]+)"));
    }

    [Fact]
    public void ExtractFieldByRegexHandleMatch()
    {
        var text = new List<string> {
            "A100",
            "B200"
        };

        Assert.Equal("200", text.ExtractFieldByRegex("B(?<value>[0-9]+)"));
    }

    [Fact]
    public void ExtractFieldByRegexHandleMissingValueInRegex()
    {
        var text = new List<string> {
            "A100",
            "B200"
        };

        Assert.Throws<ArgumentException>(() => text.ExtractFieldByRegex("B([0-9]+)"));
    }
}
