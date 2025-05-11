using System;

namespace PdfParser;

public class Logger(bool isVerbose)
{
    public bool IsVerbose { get; init; } = isVerbose;

    public void Log(string message)
    {
        if (IsVerbose)
        {
            Console.WriteLine(message);
        }
    }

    public void Log(Exception ex)
    {
        if (IsVerbose)
        {
            Console.WriteLine(ex);
        }
    }
}
