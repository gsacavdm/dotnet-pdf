using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test.StringExtensionTests;

public class ToTitleCaseTests
{
    [Fact]
    public void ToTitleCaseHandleAllUpper() => Assert.Equal("Pedro Jaimes", "PEDRO JAIMES".ToTitleCase());
    [Fact]
    public void ToTitleCaseHandleAllLower() => Assert.Equal("Pedro Jaimes", "pedro jaimes".ToTitleCase());
}
