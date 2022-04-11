using CommandLine;

namespace Sc.Pdf;

public class Options
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { set; get; }

    [Option('p', "pdfPath", Required = true, HelpText = "Path to source PDF file.")]
    public string PdfPath { set; get; }

    [Option('m', "mode", Default = "Simple", HelpText = "One of 'Simple' (Default), 'Regence', 'Premera'.")]
    public string Mode { set; get; }
}
