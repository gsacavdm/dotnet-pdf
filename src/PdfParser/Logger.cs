using System;

namespace PdfParser;

public class Logger
{
    public bool IsVerbose { get; init; }

    public Logger(bool isVerbose)
    {
        this.IsVerbose = isVerbose;
    }

    public void Log(string message)
    {
        if (this.IsVerbose)
        {
            Console.WriteLine(message);
        }
    }

    public void Log(Exception ex)
    {
        if (this.IsVerbose)
        {
            Console.WriteLine(ex);
        }
    }
}
