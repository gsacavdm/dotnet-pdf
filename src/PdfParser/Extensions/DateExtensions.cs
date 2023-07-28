using System;
using System.Globalization;

namespace PdfParser.Extensions;

public static class DateExtensions
{
    public static string ToDateString(this DateTime? dateTime) => dateTime == null ? "" : dateTime.Value.ToString("yyy-MM-dd", CultureInfo.CurrentCulture);
    public static string ToShortDateString(this DateTime? dateTime) => dateTime == null ? "" : dateTime.Value.ToString("yyyMMdd", CultureInfo.CurrentCulture);
}
