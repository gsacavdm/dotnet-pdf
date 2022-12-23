namespace Sc.Pdf.Models;

public interface IClaim
{
    public string FileName { get; }
    public bool IsValid { get; }

    public void WriteLine();
    public void WriteCsv();
    public void WriteCsvHeader();
}
