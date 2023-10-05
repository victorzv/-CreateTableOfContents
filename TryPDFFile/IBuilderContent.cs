namespace TryPDFFile;

public interface IBuilderContent
{
    TableOfContentsNode build(List<ItemInfo> list);
}