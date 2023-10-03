namespace TryPDFFile;

public class TestTree
{
    static void Main(string[] args)
    { 
      
        var listOfTexts = new List<(int FontSize, int PageNumber, int VerticalPosition, int absolutePosition)>
        {
            (33,1,900,0),
            (31,1,890,0),
            (27,1,870,0),
            (27,1,860,0),
            (24,1,850,0),
            (27,1,700,0),
            (24,1,650,0),
            (22,1,500,0),
            (22,1,400,0),
            (18,1,350,0),
            (18,1,210,0),
            (14,1,100,0),
            (14,2,900,0),
            (18,2,700,0),
            (18,2,600,0),
            (14,2,500,0),
            (12,2,400,0),
            (10,2,300,0),
            (10,2,100,0),
            (12,3,900,0),
            (10,3,800,0),
            (8,3,700,0),
            (6,3,600,0),
            (6,3,500,0),
            (8,3,400,0),
            (10,3,300,0),
            (27,3,250,0),
            (24,3,150,0),
            (22,4,900,0),
            (31,4,800,0),
            (27,4,700,0),
            (24,4,600,0),
            (22,4,500,0),
            (33,5,900,0),
            (31,5,600,0),
            (27,6,900,0),
            (24,6,750,0),
            (22,6,500,0),
            (24,6,250,0),
            (22,7,900,0),
            (31,7,600,0),
            (27,7,500,0),
            (24,7,400,0),
            (22,7,300,0),
            (18,7,150,0),
            (14,8,900,0),
            (12,8,600,0),
            (10,8,500,0),
            (8,8,400,0),
            (10,8,100,0),
            (14,9,900,0),
            (31,9,100,0)
        };

        var distinctFontSizes = listOfTexts.Select(element => element.FontSize).Distinct().ToList();

        var fontSizeLevels = distinctFontSizes
            .OrderByDescending(fontSize => fontSize) // Сортируем размеры шрифтов по убыванию.
            .Select((fontSize, index) => new { FontSize = fontSize, Level = index + 1 })
            .ToDictionary(item => item.FontSize, item => item.Level);

        
        int maxVerticalPosition = listOfTexts.Max(element => element.VerticalPosition);
        
        listOfTexts.ForEach(text => text.absolutePosition = text.PageNumber * maxVerticalPosition + text.VerticalPosition);
        
        // Сортировка списка по размеру шрифта
        var sortedList = listOfTexts
            .OrderByDescending(item => item.FontSize)
            /*.ThenBy(item => item.PageNumber)
            .ThenByDescending(item => item.VerticalPosition)*/
            .ToList();
            
        foreach (var val in sortedList)
        {
            Console.WriteLine($"FS = {val.FontSize} Page = {val.PageNumber} PagePosition = {val.VerticalPosition}");
        }
      
       // Создаем корневой узел с очень большим шрифтом.
        var tocRoot = new TableOfContentsNode(int.MaxValue,  0, 0, 0);

        foreach (var (fontSize, pageNumber, verticalPosition, absolutePosition) in sortedList)
        {
            // Находим узел, к которому нужно добавить текущий фрагмент в оглавление.
            var parentNode = FindParentNode(tocRoot, fontSize, pageNumber, verticalPosition, absolutePosition);
            
            //Console.WriteLine($"FS = {fontSize} Page = {pageNumber} PagePosition = {verticalPosition} Parent is {parentNode.PageNumber} {parentNode.FontSize} {parentNode.VerticalPosition}");

            // Создаем новый узел для текущего фрагмента.
            var newNode = new TableOfContentsNode(fontSize, pageNumber, verticalPosition, absolutePosition);

            // Добавляем текущий узел к родительскому узлу.
            parentNode.AddChild(newNode);

        }
        

        // Обход и вывод дерева оглавления.
        tocRoot.Traverse(node => Console.WriteLine($"{new string(' ', node.Depth * 4)}Font {node.FontSize} Page {node.PageNumber}, position {node.VerticalPosition}"));
        
    }

    // Метод для поиска родительского узла для текущего элемента в оглавлении.
    private static TableOfContentsNode FindParentNode(TableOfContentsNode currentNode, int fontSize, int pageNumber, int verticalPosition, int absolutePosition)
    {
        // получаем детей от текущего узла
        var sortedChildren = currentNode.Children.OrderBy(child => child.PageNumber);
        
        foreach (var child in sortedChildren)
        {
            if (child.FontSize > fontSize)
            {
                return FindParentNode(child, fontSize, pageNumber, verticalPosition, absolutePosition);    
            }
        }
        return currentNode;        
    }

}