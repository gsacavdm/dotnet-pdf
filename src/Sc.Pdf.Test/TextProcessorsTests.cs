using Xunit;
using Sc.Pdf.TextProcessors;
using Sc.Pdf.Documents;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Sc.Pdf.Extensions;

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

        // To avoid CS8602: Dereference of a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);
        Assert.Equal(CignaClaim.CignaClaimType.EoB, claim.ClaimType);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Discount").GetNullableDouble(), claim.Discount);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);

        Assert.Equal(expected.RootElement.GetProperty("AmountNotCovered").GetNullableDouble(), claim.AmountNotCovered);
        Assert.Equal(expected.RootElement.GetProperty("AllowedAmount").GetNullableDouble(), claim.AllowedAmount);
        Assert.Equal(expected.RootElement.GetProperty("Copay").GetNullableDouble(), claim.Copay);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetNullableDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("AmountPaid").GetNullableDouble(), claim.AmountPaid);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetNullableDouble(), claim.Coinsurance);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }

    [Fact]
    public async Task CignaWebClaimProcessor()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/cignaweb.pdf");
        using var file = File.Open("./asserts/cignaweb.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new CignaWebClaimProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var claim = document as CignaClaim;
        Assert.NotNull(claim);

        // To avoid CS8602: Dereference of a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);
        Assert.Equal(CignaClaim.CignaClaimType.Web, claim.ClaimType);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Discount").GetNullableDouble(), claim.Discount);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);

        Assert.Equal(expected.RootElement.GetProperty("AmountNotCovered").GetNullableDouble(), claim.AmountNotCovered);
        Assert.Equal(expected.RootElement.GetProperty("AllowedAmount").GetNullableDouble(), claim.AllowedAmount);
        Assert.Equal(expected.RootElement.GetProperty("Copay").GetNullableDouble(), claim.Copay);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetNullableDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("AmountPaid").GetNullableDouble(), claim.AmountPaid);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetNullableDouble(), claim.Coinsurance);

        Assert.Equal(expected.RootElement.GetProperty("StandardFileName").GetString(), claim.StandardFileName);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }

    [Fact]
    public async Task CignaClaimMoreInfoNeededProcessor()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/cigna-moreinfo.pdf");
        using var file = File.Open("./asserts/cigna-moreinfo.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new CignaMoreInfoNeededClaimProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var claim = document as CignaClaim;
        Assert.NotNull(claim);

        // To avoid CS8602: Dereference of a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);
        Assert.Equal(CignaClaim.CignaClaimType.MoreInfoNeeded, claim.ClaimType);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Discount").GetNullableDouble(), claim.Discount);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);

        Assert.Equal(expected.RootElement.GetProperty("AmountNotCovered").GetNullableDouble(), claim.AmountNotCovered);
        Assert.Equal(expected.RootElement.GetProperty("AllowedAmount").GetNullableDouble(), claim.AllowedAmount);
        Assert.Equal(expected.RootElement.GetProperty("Copay").GetNullableDouble(), claim.Copay);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetNullableDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("AmountPaid").GetNullableDouble(), claim.AmountPaid);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetNullableDouble(), claim.Coinsurance);

        Assert.Equal(expected.RootElement.GetProperty("StandardFileName").GetString(), claim.StandardFileName);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }
    [Fact]
    public async Task CignaClaimMoreInfoNeededProcessor2()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/cigna-moreinfo2.pdf");
        using var file = File.Open("./asserts/cigna-moreinfo2.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new CignaMoreInfoNeededClaimProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var claim = document as CignaClaim;
        Assert.NotNull(claim);

        // To avoid CS8602: Dereference of a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);
        Assert.Equal(CignaClaim.CignaClaimType.MoreInfoNeeded, claim.ClaimType);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Discount").GetNullableDouble(), claim.Discount);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);

        Assert.Equal(expected.RootElement.GetProperty("AmountNotCovered").GetNullableDouble(), claim.AmountNotCovered);
        Assert.Equal(expected.RootElement.GetProperty("AllowedAmount").GetNullableDouble(), claim.AllowedAmount);
        Assert.Equal(expected.RootElement.GetProperty("Copay").GetNullableDouble(), claim.Copay);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetNullableDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("AmountPaid").GetNullableDouble(), claim.AmountPaid);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetNullableDouble(), claim.Coinsurance);

        Assert.Equal(expected.RootElement.GetProperty("StandardFileName").GetString(), claim.StandardFileName);

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

        // To avoid CS8602: Dereference of a possibly null ref
        if (claim == null)
        {
            return;
        }

        Assert.True(claim.IsValid);
        Assert.Null(claim.ParseException);

        Assert.Equal(expected.RootElement.GetProperty("ClaimNumber").GetString(), claim.ClaimNumber);
        Assert.Equal(expected.RootElement.GetProperty("Coinsurance").GetNullableDouble(), claim.Coinsurance);
        Assert.Equal(expected.RootElement.GetProperty("DateOfService").GetDateTime(), claim.DateOfService);
        Assert.Equal(expected.RootElement.GetProperty("DateProcessed").GetDateTime(), claim.DateProcessed);
        Assert.Equal(expected.RootElement.GetProperty("Deductible").GetNullableDouble(), claim.Deductible);
        Assert.Equal(expected.RootElement.GetProperty("FromAnotherSource").GetNullableDouble(), claim.FromAnotherSource);
        Assert.Equal(expected.RootElement.GetProperty("MemberName").GetString(), claim.MemberName);
        Assert.Equal(expected.RootElement.GetProperty("NetworkDiscount").GetNullableDouble(), claim.NetworkDiscount);
        Assert.Equal(expected.RootElement.GetProperty("NotCovered").GetNullableDouble(), claim.NotCovered);
        Assert.Equal(expected.RootElement.GetProperty("PaidByHealthPlan").GetNullableDouble(), claim.PaidByHealthPlan);
        Assert.Equal(expected.RootElement.GetProperty("ProviderName").GetString(), claim.ProviderName);
        Assert.Equal(expected.RootElement.GetProperty("TotalPlanDiscountsAndPayments").GetNullableDouble(), claim.TotalPlanDiscountsAndPayments);

        Assert.Equal(expected.RootElement.GetProperty("StandardFileName").GetString(), claim.StandardFileName);

        Assert.Equal(text, claim.SourceText);
        Assert.Null(claim.SourceFileName);
    }

    [Fact]
    public async Task TruistStatementProcessor()
    {
        var text = PdfParsers.PdfParser.Parse("./pdfs/truist.pdf");
        using var file = File.Open("./asserts/truist.json", FileMode.Open);
        var expected = await JsonDocument.ParseAsync(file);

        var processor = new TruistStatementProcessor();
        Assert.True(processor.TryParse(text, out var document));

        var statement = document as TruistStatement;
        Assert.NotNull(statement);

        // To avoid CS8602: Dereference of a possibly null ref
        if (statement == null)
        {
            return;
        }

        Assert.True(statement.IsValid);
        Assert.Null(statement.ParseException);

        Assert.Equal(expected.RootElement.GetProperty("DueDate").GetDateTime(), statement.DueDate);
        Assert.Equal(expected.RootElement.GetProperty("StatementDate").GetDateTime(), statement.StatementDate);
        Assert.Equal(expected.RootElement.GetProperty("PrincipalBalance").GetNullableDouble(), statement.PrincipalBalance);
        Assert.Equal(expected.RootElement.GetProperty("EscrowBalance").GetNullableDouble(), statement.EscrowBalance);
        Assert.Equal(expected.RootElement.GetProperty("AmountDue").GetNullableDouble(), statement.AmountDue);
        Assert.Equal(expected.RootElement.GetProperty("PrincipalDue").GetNullableDouble(), statement.PrincipalDue);
        Assert.Equal(expected.RootElement.GetProperty("InterestDue").GetNullableDouble(), statement.InterestDue);
        Assert.Equal(expected.RootElement.GetProperty("EscrowTaxesAndInsurance").GetNullableDouble(), statement.EscrowTaxesAndInsurance);

        Assert.Equal(expected.RootElement.GetProperty("StandardFileName").GetString(), statement.StandardFileName);

        Assert.Equal(text, statement.SourceText);
        Assert.Null(statement.SourceFileName);
    }
}
