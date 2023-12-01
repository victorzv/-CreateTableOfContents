using TryPDFFile.Model;

namespace TryPDFFile.Service.Interface;

public interface ITableContentBuilder
{
    Chapter build(List<ParagraphInfo> list);
}