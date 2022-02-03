using System;
using CommandLine;

namespace dotnet_pdf
{
  public class Options
  {
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { set; get; }

    [Option('p', "pdfPath", Required = true, HelpText = "Path to source PDF file.")]
    public string PdfPath { set; get; }
  }
}
