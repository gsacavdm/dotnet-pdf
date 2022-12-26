using System.Text.Json;

namespace Sc.Pdf.Extensions;

public static class JsonElementExtensions
{
    public static double? GetNullableDouble(this JsonElement jsonElement)
        => jsonElement.ValueKind == JsonValueKind.Null ? null : jsonElement.GetDouble();
}
