using System;
using System.Globalization;

namespace Sc.Pdf.Extensions;

public static class DateExtensions
{
    public static string ToDateString(this DateTime? dateTime) => dateTime == null ? "" : dateTime.Value.ToString("yyy-MM-dd", CultureInfo.CurrentCulture);
}