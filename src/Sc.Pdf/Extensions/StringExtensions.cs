using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sc.Pdf.Extensions;

public static class StringExtensions
{
    public static string ToTitleCase(this string s) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower(CultureInfo.InvariantCulture));

    public static double? ParseDouble(this string doubleString)
    {
        doubleString = doubleString.Replace("$", "").Replace(",", "");
        var couldParse = double.TryParse(doubleString, out var doubleVal);
        return couldParse ? doubleVal : null;
    }

    public static DateTime? ParseDate(this string dateText)
    {
        var couldParse = DateTime.TryParse(dateText, out var dateTime);
        return couldParse ? dateTime : null;
    }
    public static string ExtractFieldByPosition(this IEnumerable<string> textRows, int position)
    {
        var textRow = textRows.ElementAtOrDefault(position);

        return textRow ?? string.Empty;
    }

    public static string ExtractFieldNextLineByEquals(this IEnumerable<string> textRows, string searchText)
    {
        var searchLine = textRows.FirstOrDefault(line => line.Equals(searchText, StringComparison.Ordinal));
        if (searchLine == null)
        {
            return string.Empty;
        }

        var searchPosition = Array.IndexOf(textRows.ToArray(), searchLine);
        var textRow = textRows.ElementAtOrDefault(searchPosition + 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldNextLineByStartsWith(this IEnumerable<string> textRows, string searchText)
    {
        var searchLine = textRows.FirstOrDefault(line => line.StartsWith(searchText, StringComparison.Ordinal));
        if (searchLine == null)
        {
            return string.Empty;
        }

        var searchPosition = Array.IndexOf(textRows.ToArray(), searchLine);
        var textRow = textRows.ElementAtOrDefault(searchPosition + 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldPreviousLineByEquals(this IEnumerable<string> textRows, string searchText)
    {
        var searchLine = textRows.FirstOrDefault(line => line.Equals(searchText, StringComparison.Ordinal));
        if (searchLine == null)
        {
            return string.Empty;
        }

        var searchPosition = Array.IndexOf(textRows.ToArray(), searchLine);
        var textRow = textRows.ElementAtOrDefault(searchPosition - 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldPreviousLineByStartsWith(this IEnumerable<string> textRows, string searchText)
    {
        var searchLine = textRows.FirstOrDefault(line => line.StartsWith(searchText, StringComparison.Ordinal));
        if (searchLine == null)
        {
            return string.Empty;
        }

        var searchPosition = Array.IndexOf(textRows.ToArray(), searchLine);
        var textRow = textRows.ElementAtOrDefault(searchPosition - 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldByStartsWith(this IEnumerable<string> textRows, string searchText)
    {
        var textRow = textRows.FirstOrDefault(s => s.StartsWith(searchText, StringComparison.InvariantCulture));

        if (textRow == null)
        {
            return string.Empty;
        }

        textRow = textRow.Replace(searchText, "").Trim();
        return textRow;
    }


    /// <summary>
    /// Extracts text from the first line which matches the regex provided.
    /// </summary>
    /// <param name="regex">Regex used to pattern match. Must have the &lt;value&gt; token to indicate which value should be extracted.</param>
    public static string ExtractFieldByRegex(this IEnumerable<string> textRows, string regex)
    {
        if (!regex.Contains("<value>"))
        {
            throw new ArgumentException("Regex must contain <value> token to indicate value to be returned.");
        }

        var textRow = textRows.FirstOrDefault(s => Regex.IsMatch(s, regex));
        return textRow == null ? string.Empty : Regex.Match(textRow, regex).Groups["value"].Value;
    }
}
