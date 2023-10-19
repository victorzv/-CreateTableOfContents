namespace TryPDFFile;

public interface IBuildData
{
    List<ItemInfo> parseData(string pdfDocPath);
}