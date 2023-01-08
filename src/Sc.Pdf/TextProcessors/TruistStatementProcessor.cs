using System;
using System.Collections.Generic;
using Sc.Pdf.Documents;
using Sc.Pdf.Extensions;

namespace Sc.Pdf.TextProcessors;

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

            //ToDo
            //truistStatement.PrincipalBalance = text.ExtractFieldNextLineByEquals("Principal balance").ParseDouble();

            truistStatement.EscrowBalance = text.ExtractFieldByStartsWith("Escrow balance").ParseDouble();
            truistStatement.AmountDue = text.ExtractFieldByStartsWith("Amount Due:").ParseDouble();

            //ToDo
            //truistStatement.PrincipalDue = text.ExtractFieldByStartsWith("Principal").ParseDouble();
            //truistStatement.InterestDue = text.ExtractFieldByStartsWith("Interest").ParseDouble();

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
