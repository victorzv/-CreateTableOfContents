using TryPDFFile.Model;

namespace PdfOutliner.Service.Interface
{
    public interface ITableContentFilter
    {
        List<ParagraphInfo> Reduce(List<ParagraphInfo> list, int levels);
    }
}
