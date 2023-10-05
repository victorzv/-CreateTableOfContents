namespace TryPDFFile;

public class GenNormalContent : IBuilderContent
{
    public TableOfContentsNode build(List<ItemInfo> list)
    {
        // фэйковый корень
        TableOfContentsNode root = new TableOfContentsNode(Int32.MaxValue, -10, -10, -10, 0);
        
        double maxVerticalPosition = list.Max(el => el.VerticalPosition);
        
        list.ForEach(el=>el.AbsolutePosition = el.Page * maxVerticalPosition - el.VerticalPosition);
        
        var groupedByFontSize = list
            .OrderByDescending(element => element.FontSize)
            .GroupBy(element => element.FontSize)
            .ToDictionary(group => group.Key, group => group.ToList());
        
        double previousKey = groupedByFontSize.First().Key; 
        var previousElements = groupedByFontSize.First().Value;
        
        // Добавляем первую группу, как детей в фейковый корень
        foreach (var element in previousElements)
        {
            var newNode = new TableOfContentsNode(element.FontSize, element.VerticalPosition, element.Page, element.AbsolutePosition);
            root.AddChild(newNode);
        }

        foreach (var kvp in groupedByFontSize.Skip(1))
        {
            double fontSize = kvp.Key;
            List<ItemInfo> currentElements = kvp.Value;

            TableOfContentsNode previousNode = null; // Создаем предыдущий узел

            foreach (var element in currentElements)
            {
                previousNode = null;
                previousElements = previousElements.OrderByDescending(el => el.AbsolutePosition).ToList();
                foreach (var t in previousElements)
                {
                    if (element.AbsolutePosition > t.AbsolutePosition)
                    {
                        previousNode = root.FindNode(t.AbsolutePosition);        
                        break;
                    }
                }

                if (previousNode == null) continue;
                var newNode = new TableOfContentsNode(element.FontSize, element.VerticalPosition, element.Page, element.AbsolutePosition);
                previousNode.AddChild(newNode);
            }

            // Записываем текущую группу как предыдущую для следующей итерации
            previousElements = currentElements;
        }


        return root;
    }
}