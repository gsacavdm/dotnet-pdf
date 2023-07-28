using System.Text.Json;
using PdfParser.Extensions;
using Xunit;

namespace PdfParser.Test;

public class JsonElementExtensionTest
{
    [Fact]
    public void GetNullableDoubleHandlesNull()
    {
        var obj = new
        {
            Property = (double?)null
        };
        var doc = JsonSerializer.SerializeToDocument(obj);

        Assert.Null(doc.RootElement.GetProperty("Property").GetNullableDouble());
    }

    [Fact]
    public void GetNullableDoubleHandlesDouble()
    {
        var obj = new
        {
            Property = 1.0
        };
        var doc = JsonSerializer.SerializeToDocument(obj);

        Assert.Equal(1.0, doc.RootElement.GetProperty("Property").GetNullableDouble());
    }
}
