namespace TryPDFFile;

public class TestTree
{
    static void Main(string[] args)
    { 
      
        var listOfTexts = new List<(int FontSize, int VerticalPosition, int PageNumber, int absolutePosition)>
        {

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

        foreach (var (fontSize, verticalPosition, pageNumber, absolutePosition) in sortedList)
        {
            // Находим узел, к которому нужно добавить текущий фрагмент в оглавление.
            var parentNode = FindParentNode(tocRoot, fontSize, verticalPosition, pageNumber, absolutePosition);
            
            //Console.WriteLine($"FS = {fontSize} Page = {pageNumber} PagePosition = {verticalPosition} Parent is {parentNode.PageNumber} {parentNode.FontSize} {parentNode.VerticalPosition}");

            // Создаем новый узел для текущего фрагмента.
            var newNode = new TableOfContentsNode(fontSize, verticalPosition, pageNumber, absolutePosition);

            // Добавляем текущий узел к родительскому узлу.
            parentNode.AddChild(newNode);

        }
        

        // Обход и вывод дерева оглавления.
        tocRoot.Traverse(node => Console.WriteLine($"{new string(' ', node.Depth * 4)}Font {node.FontSize} Page {node.PageNumber}, position {node.VerticalPosition}"));
        
    }

    // Метод для поиска родительского узла для текущего элемента в оглавлении.
    private static TableOfContentsNode FindParentNode(TableOfContentsNode currentNode, int fontSize, int verticalPosition, int pageNumber, int absolutePosition)
    {
        // получаем детей от текущего узла
        var sortedChildren = currentNode.Children.OrderBy(child => child.PageNumber);
        
        foreach (var child in sortedChildren)
        {
            if (child.FontSize > fontSize)
            {
                return FindParentNode(child, fontSize, verticalPosition, pageNumber, absolutePosition);    
            }
        }

        
        
        return currentNode;        
    }

    private static List<TableOfContentsNode> getNextChilds(TableOfContentsNode parent, int fontSize, int pageNumber)
    {
        var matchingChilds = parent.Children.FindAll(child => child.FontSize > fontSize && pageNumber == child.PageNumber);
        return matchingChilds;
    }
}