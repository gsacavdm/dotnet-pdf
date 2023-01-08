using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sc.Pdf.Documents;

public abstract class DocumentBase
{
    [CsvIgnore]
    public IEnumerable<string> SourceText { get; set; }

    [CsvIgnore]
    public string SourceFileName { get; set; }
    [CsvIgnore]
    public Exception ParseException { get; set; }
    [CsvIgnore]
    public string StandardFileName => GetStandardFileName();
    [CsvIgnore]
    public bool IsValid => GetIsValid();

    private readonly IWriter _writer;

    protected abstract string GetStandardFileName();
    protected abstract bool GetIsValid();

    public DocumentBase(IWriter writer = null)
    {
        _writer = writer;
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        var properties = this.GetType().GetProperties();
        var filteredProperties = properties
            .Where(p =>
                !p.GetCustomAttributes(true).Any(
                    a => a.GetType().Name.Equals("CsvIgnoreAttribute", StringComparison.Ordinal)
                )
            );
        return filteredProperties;
    }

    public void WriteCsvHeader()
    {
        var properties = this.GetProperties();
        var propertyNames = properties.Select(p => p.Name);
        var output = String.Join(",", propertyNames);

        WriteLine(output);
    }

    public void WriteCsv()
    {
        var properties = this.GetProperties();
        var propertyValues = properties
            .Select(p => "\"" + p.GetValue(this)?.ToString()?.Replace("\"", "\"\"") + "\"");
        var output = String.Join(",", propertyValues);

        WriteLine(output);
    }

    public void WriteText()
    {
        var properties = this.GetProperties();
        var propertyValues = properties
            .Select(p => p.GetType().Name + ": " + p.GetValue(this).ToString());
        var output = String.Join("\n", propertyValues);

        WriteLine(output);
    }

    private void WriteLine(string output)
    {
        if (_writer != null)
        {
            _writer.WriteLine(output);
        }
        else
        {
            Console.WriteLine(output);
        }
    }
}
