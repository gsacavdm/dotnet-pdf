using CommandLine;

namespace Sc.Pdf;

public class Options
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { set; get; }

    [Option('f', "pdfFilePath", HelpText = "Path to source PDF file.")]
    public string PdfFilePath { set; get; }

    [Option('d', "pdfDirectoryPath", HelpText = "Path to directory with source PDF files.")]
    public string PdfDirectoryPath { set; get; }

    [Option('p', "pdfFilePattern", Required = false, HelpText = "File pattern used to limit which files to process when using the pdfDirectoryPath / -p parameter.")]
    public string PdfDirectoryFilePattern { set; get; }

    [Option('m', "mode", Default = "Simple", HelpText = "One of 'Simple' (Default), 'Csv', 'Move'.")]
    public string Mode { set; get; }

    [Option('o', "overwrite", Required = false, HelpText = "Overwrite file if exists. Applicable only for move operations.")]
    public bool Overwrite { set; get; }
}
