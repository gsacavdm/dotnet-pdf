using System;

namespace PdfParser;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class CsvIgnoreAttribute : Attribute
{
}
