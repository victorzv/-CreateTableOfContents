using System.Net.Mime;

namespace TryPDFFile;

public class TestTree
{
    static void Main(string[] args)
    {

        var listOfTexts = new List<(int FontSize, int PageNumber, int VerticalPosition, int AbsolutePosition)>
        {
            (33, 1, 900, 0),
            (31, 1, 890, 0),
            (27, 1, 870, 0),
            (27, 1, 860, 0),
            (24, 1, 850, 0),
            (27, 1, 700, 0),
            (24, 1, 650, 0),
            (22, 1, 500, 0),
            (22, 1, 400, 0),
            (18, 1, 350, 0),
            (18, 1, 210, 0),
            (14, 1, 100, 0),
            (14, 2, 900, 0),
            (18, 2, 700, 0),
            (18, 2, 600, 0),
            (14, 2, 500, 0),
            (12, 2, 400, 0),
            (10, 2, 300, 0),
            (10, 2, 100, 0),
            (12, 3, 900, 0),
            (10, 3, 800, 0),
            (8, 3, 700, 0),
            (6, 3, 600, 0),
            (6, 3, 500, 0),
            (8, 3, 400, 0),
            (10, 3, 300, 0),
            (27, 3, 250, 0),
            (24, 3, 150, 0),
            (22, 4, 900, 0),
            (31, 4, 800, 0),
            (27, 4, 700, 0),
            (24, 4, 600, 0),
            (22, 4, 500, 0),
            (33, 5, 900, 0),
            (31, 5, 600, 0),
            (27, 6, 900, 0),
            (24, 6, 750, 0),
            (22, 6, 500, 0),
            (24, 6, 250, 0),
            (22, 7, 900, 0),
            (31, 7, 600, 0),
            (27, 7, 500, 0),
            (24, 7, 400, 0),
            (22, 7, 300, 0),
            (18, 7, 150, 0),
            (14, 8, 900, 0),
            (12, 8, 600, 0),
            (10, 8, 500, 0),
            (8, 8, 400, 0),
            (10, 8, 100, 0),
            (14, 9, 900, 0),
            (31, 9, 100, 0)
        };

        int maxVerticalPosition = listOfTexts.Max(el => el.VerticalPosition);
        
        var updatedList = listOfTexts.Select(element =>
        {
            int newAbsolutePosition = element.PageNumber * maxVerticalPosition - element.VerticalPosition;
            return (element.FontSize, element.PageNumber, element.VerticalPosition, newAbsolutePosition);
        }).ToList();

        listOfTexts = updatedList;
        
        // Создаем фейковый корень с очень большим шрифтом
        var root = new TableOfContentsNode(int.MaxValue, 1000, -10, -10);
        root.Depth = 0;
        
        // Группируем данные по размеру шрифта и сортируем
        var groupedByFontSize = listOfTexts
            .OrderByDescending(element => element.FontSize)
            .GroupBy(element => element.FontSize)
            .ToDictionary(group => group.Key, group => group.ToList());

        // Получаем первую группу (самый большой шрифт)
        int previousKey = groupedByFontSize.First().Key; 
        var previousElements = groupedByFontSize.First().Value;

        // Добавляем первую группу, как детей в фейковый корень
        foreach (var element in previousElements)
        {
            var newNode = new TableOfContentsNode(element.FontSize, element.VerticalPosition, element.PageNumber, element.AbsolutePosition);
            root.AddChild(newNode);
        }
        
        foreach (var kvp in groupedByFontSize.Skip(1))
        {
            int fontSize = kvp.Key;
            List<(int FontSize, int PageNumber, int VerticalPosition, int AbsolutePosition)> currentElements = kvp.Value;

            TableOfContentsNode previousNode = null; // Создаем предыдущий узел

            foreach (var element in currentElements)
            {
                previousNode = null;
                previousElements = previousElements.OrderByDescending(el => el.AbsolutePosition).ToList();
                for (int i = 0; i < previousElements.Count; i++)
                {
                    if (element.AbsolutePosition > previousElements[i].AbsolutePosition)
                    {
                        previousNode = root.FindNode(previousElements[i].AbsolutePosition);        
                        break;
                    }
                }

                if (previousNode != null)
                {
                    var newNode = new TableOfContentsNode(element.FontSize, element.VerticalPosition, element.PageNumber, element.AbsolutePosition);
                    previousNode.AddChild(newNode);
                }
            }

            // Записываем текущую группу как предыдущую для следующей итерации
            previousElements = currentElements;
        }
        
        root.Traverse(el=>Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition}"));

    }
}