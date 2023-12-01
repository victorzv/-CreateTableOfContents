using TryPDFFile.Model;

namespace TryPDFFile.Service.Interface;

public interface IPdfParser
{
    List<ParagraphInfo> parseData(string pdfDocPath);
}