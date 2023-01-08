using Sc.Pdf.Extensions;
using Xunit;

namespace Sc.Pdf.Test.StringExtensionTests;

public class ToTitleCaseTests
{
    [Fact]
    public void ToTitleCaseHandleAllUpper() => Assert.Equal("Pedro Jaimes", "PEDRO JAIMES".ToTitleCase());
    [Fact]
    public void ToTitleCaseHandleAllLower() => Assert.Equal("Pedro Jaimes", "pedro jaimes".ToTitleCase());
}
