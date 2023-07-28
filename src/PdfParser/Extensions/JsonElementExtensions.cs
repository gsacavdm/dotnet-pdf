using System.Text.Json;

namespace PdfParser.Extensions;

public static class JsonElementExtensions
{
    public static double? GetNullableDouble(this JsonElement jsonElement)
        => jsonElement.ValueKind == JsonValueKind.Null ? null : jsonElement.GetDouble();
}
