namespace TryPDFFile;

public interface ITableContentBuilder
{
    Chapter build(List<IParagraphInfo> list);
}