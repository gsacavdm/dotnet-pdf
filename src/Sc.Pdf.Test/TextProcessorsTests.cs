using Xunit;
using Sc.Pdf.TextProcessors;
using Sc.Pdf.Documents;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sc.Pdf.Test;

public class TextProcessorsTests
{
    [Fact]
    public async Task CignaClaimProcessor()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/cigna.pdf");
        using var file = File.Open("./asserts/cigna.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new CignaClaimProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var claim = document as CignaClaim;
        Assert.NotNull(claim);

        // To avoid CS8602: Dereferenceof a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Discount").GetDouble(), claim.Discount);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }
    [Fact]
    public async Task PremeraClaimProcessor()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/premera.pdf");
        using var file = File.Open("./asserts/premera.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new PremeraClaimProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var claim = document as PremeraClaim;
        Assert.NotNull(claim);

        // To avoid CS8602: Dereferenceof a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetDouble(), claim.Coinsurance);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("FromAnotherSource").GetDouble(), claim.FromAnotherSource);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("NetworkDiscount").GetDouble(), claim.NetworkDiscount);
        Assert.Equal(expected.RootElement.GetProperty("NotCovered").GetDouble(), claim.NotCovered);
        Assert.Equal(expected.RootElement.GetProperty("PaidByHealthPlan").GetDouble(), claim.PaidByHealthPlan);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);
        Assert.Equal(expected.RootElement.GetProperty("TotalPlanDiscountsAndPayments").GetDouble(), claim.TotalPlanDiscountsAndPayments);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }
}
