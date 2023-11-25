namespace TryPDFFile;

public interface IPdfParser
{
    List<IParagraphInfo> parseData(string pdfDocPath);
}