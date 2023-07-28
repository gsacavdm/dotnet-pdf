using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ParseDoubleTests
{
    [Fact]
    public void ParseDoubleHandleValidNumber() => Assert.Equal(2.5, "2.5".ParseDouble());
    [Fact]
    public void ParseDoubleHandleCurrency() => Assert.Equal(2000.50, "$2,000.50".ParseDouble());
    [Fact]
    public void ExtractFieldByStartsWithFindMatch() => Assert.Null("Text Only".ParseDouble());
}
