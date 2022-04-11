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

    [Option('m', "mode", Default = "Simple", HelpText = "One of 'Simple' (Default), 'Regence', 'Premera'.")]
    public string Mode { set; get; }
}
