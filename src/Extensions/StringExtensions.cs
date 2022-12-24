using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
}
