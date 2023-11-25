namespace TryPDFFile;

public class GenNormalContent : IBuilderContent
{
    public TableOfContentsNode build(List<ItemInfo> list)
    {
        // фэйковый корень
        TableOfContentsNode root = new TableOfContentsNode(Int32.MaxValue, -10, -10, "");
        
        var groupedByFontSize = list
            .OrderByDescending(element => element.FontSize)
            .GroupBy(element => element.FontSize)
            .ToDictionary(group => group.Key, group => group.ToList());
        
        double previousKey = groupedByFontSize.First().Key; 
        var previousElements = groupedByFontSize.First().Value;
        
        // Добавляем первую группу, как детей в фейковый корень
        foreach (var element in previousElements)
        {
            //double fontSize, int pageNumber, int absolutePosition, string text, int depth = 0
            var newNode = new TableOfContentsNode(element.FontSize, element.Page, element.AbsolutePosition, element.Text);
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
                var newNode = new TableOfContentsNode(element.FontSize, element.Page, element.AbsolutePosition, element.Text);
                previousNode.AddChild(newNode);
            }

            // Записываем текущую группу как предыдущую для следующей итерации
            previousElements = currentElements;
        }


        return root;
    }
}