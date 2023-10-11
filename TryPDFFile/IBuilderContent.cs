using System.Collections.Generic;

namespace TryPDFFile;

public interface IBuilderContent
{
    TableOfContentsNode build(List<ItemInfo> list);
}