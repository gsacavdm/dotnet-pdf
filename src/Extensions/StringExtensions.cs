namespace Sc.Pdf.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string s) => string.IsNullOrEmpty(s) ? s : s[0..1].ToUpperInvariant() + s[1..].ToLowerInvariant();
}
