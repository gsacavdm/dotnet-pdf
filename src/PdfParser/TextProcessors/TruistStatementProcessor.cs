using System;
using System.Collections.Generic;
using PdfParser.Documents;
using PdfParser.Extensions;

namespace PdfParser.TextProcessors;

public class TruistStatementProcessor : ITextProcessor
{

    public bool TryParse(IEnumerable<string> text, out IDocument document)
    {
        var parsedSuccessfully = false;
        var truistStatement = new TruistStatement
        {
            SourceText = text
        };

        try
        {
            truistStatement.DueDate = text.ExtractFieldByStartsWith("Payment Due Date:").ParseDate();
            truistStatement.StatementDate = text.ExtractFieldByStartsWith("Statement Date:").ParseDate();
            truistStatement.PrincipalBalance = text.ExtractFieldPreviousLineByStartsWith("Escrow balance").ParseDouble();
            truistStatement.EscrowBalance = text.ExtractFieldByStartsWith("Escrow balance").ParseDouble();
            truistStatement.AmountDue = text.ExtractFieldByStartsWith("Amount Due:").ParseDouble();
            truistStatement.PrincipalDue = text.ExtractFieldByStartsWith("Principal", 2).ParseDouble();
            truistStatement.InterestDue = text.ExtractFieldByStartsWith("Interest", 2).ParseDouble();

            truistStatement.EscrowTaxesAndInsurance = text.ExtractFieldByStartsWith("Escrow (taxes & insurance)").ParseDouble();

            parsedSuccessfully = true;
        }
        catch (Exception ex)
        {
            truistStatement.ParseException = ex;
        }

        document = truistStatement;
        return parsedSuccessfully && document.IsValid;
    }
}
