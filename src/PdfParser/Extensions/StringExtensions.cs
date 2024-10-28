using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PdfParser.Extensions;

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

    public static string ExtractFieldNextLineByEquals(this IEnumerable<string> textRows, string searchText, int matchNumber = 1)
    {
        var matches = textRows.Select((textRow, index) => new { TextRow = textRow, Index = index })
            .Where(s => s.TextRow.Equals(searchText, StringComparison.OrdinalIgnoreCase));
        var searchLine = matches.ElementAtOrDefault(matchNumber - 1);
        if (searchLine == null)
        {
            return string.Empty;
        }

        var textRow = textRows.ElementAtOrDefault(searchLine.Index + 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldNextLineByStartsWith(this IEnumerable<string> textRows, string searchText, int matchNumber = 1)
    {
        var matches = textRows.Select((textRow, index) => new { TextRow = textRow, Index = index })
            .Where(s => s.TextRow.StartsWith(searchText, StringComparison.InvariantCulture));
        var searchLine = matches.ElementAtOrDefault(matchNumber - 1);

        if (searchLine == null)
        {
            return string.Empty;
        }

        var textRow = textRows.ElementAtOrDefault(searchLine.Index + 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldPreviousLineByEquals(this IEnumerable<string> textRows, string searchText, int matchNumber = 1)
    {
        var matches = textRows.Select((textRow, index) => new { TextRow = textRow, Index = index })
            .Where(s => s.TextRow.Equals(searchText, StringComparison.OrdinalIgnoreCase));
        var searchLine = matches.ElementAtOrDefault(matchNumber - 1);

        if (searchLine == null)
        {
            return string.Empty;
        }

        var textRow = textRows.ElementAtOrDefault(searchLine.Index - 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldPreviousLineByStartsWith(this IEnumerable<string> textRows, string searchText, int matchNumber = 1)
    {
        var matches = textRows.Select((textRow, index) => new { TextRow = textRow, Index = index })
            .Where(s => s.TextRow.StartsWith(searchText, StringComparison.InvariantCulture));
        var searchLine = matches.ElementAtOrDefault(matchNumber - 1);

        if (searchLine == null)
        {
            return string.Empty;
        }

        var textRow = textRows.ElementAtOrDefault(searchLine.Index - 1);
        return textRow ?? string.Empty;
    }

    public static string ExtractFieldByStartsWith(this IEnumerable<string> textRows, string searchText, int matchNumber = 1)
    {
        var matches = textRows.Where(s => s.StartsWith(searchText, StringComparison.InvariantCulture));
        var textRow = matches.ElementAtOrDefault(matchNumber - 1);

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
    /// <param name="match">Indicate which match to return. Default = 1, first match.</param>
    public static string ExtractFieldByRegex(this IEnumerable<string> textRows, string regex, int match = 1)
    {
        if (!regex.Contains("<value>"))
        {
            throw new ArgumentException("Regex must contain <value> token to indicate value to be returned.");
        }

        var textRow = textRows.Where(s => Regex.IsMatch(s, regex)).ElementAtOrDefault(match - 1);
        return textRow == null ? string.Empty : Regex.Match(textRow, regex).Groups["value"].Value;
    }

    /// <summary>
    /// Removes redundant spaces from a string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string RemoveRedundantSpaces(this string s)
    {
        var regex = new Regex(@"\s+");
        return regex.Replace(s, " ").Trim();
    }
}
