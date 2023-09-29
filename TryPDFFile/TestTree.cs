namespace TryPDFFile;

public class TestTree
{
    static void Main(string[] args)
    { 
        var listOfTexts = new List<(int FontSize, string Text, int VerticalPosition, int PageNumber)>
        {
            (10, "text 10", 750, 1),
            (27, "text 27 1", 755, 1),
            (16, "text 16 1", 700, 1),
            (10, "text 10", 655, 1),
            (27, "text 27 2", 751, 2),
            (10, "text 10", 750, 2),
            (10, "text 10", 655, 2),
            (16, "text 16", 720, 2),
            (16, "text 16", 700, 2),
            (14, "text 14", 715, 2),
            (10, "text 10", 650, 2),
            (14, "text 10", 750, 3),
            (27, "text 27 1", 755, 3),
            (16, "text 16 1", 700, 3),
            (12, "text 12", 655, 3),
            (10, "text 10", 650, 3),
            (27, "text 27 2", 755, 2),
            (10, "text 10", 750, 3),
            (10, "text 10", 655, 3),
            (16, "text 16", 720, 3),
            (16, "text 16", 700, 3),
            (10, "text 10", 715, 3),
            (10, "text 10", 650, 4)
        };

        // Сортировка списка по размеру шрифта
        var sortedList = listOfTexts.OrderByDescending(item => item.FontSize)
            .ThenBy(item => item.PageNumber)
            .ThenByDescending(item => item.VerticalPosition)
            .ToList();

        foreach (var val in sortedList)
        {
            Console.WriteLine($"FS = {val.FontSize} Text = {val.Text} Page = {val.PageNumber} PagePosition = {val.VerticalPosition}");
        }
      
       // Создаем корневой узел с очень большим шрифтом.
        var tocRoot = new TableOfContentsNode(int.MaxValue, "Root", 0, 0);

        foreach (var (fontSize, text, verticalPosition, pageNumber) in sortedList)
        {
            // Находим узел, к которому нужно добавить текущий фрагмент в оглавление.
            var parentNode = FindParentNode(tocRoot, fontSize, verticalPosition, pageNumber);
            
            //Console.WriteLine($"FS = {fontSize} Page = {pageNumber} PagePosition = {verticalPosition} Parent is {parentNode.PageNumber} {parentNode.FontSize} {parentNode.VerticalPosition}");

            // Создаем новый узел для текущего фрагмента.
            var newNode = new TableOfContentsNode(fontSize, text, verticalPosition, pageNumber);

            // Добавляем текущий узел к родительскому узлу.
            parentNode.AddChild(newNode);

        }
        

        // Обход и вывод дерева оглавления.
        tocRoot.Traverse(node => Console.WriteLine($"{new string(' ', node.Depth * 4)}Font {node.FontSize} Page {node.PageNumber}, position {node.VerticalPosition}"));
        
    }

    // Метод для поиска родительского узла для текущего элемента в оглавлении.
    private static TableOfContentsNode FindParentNode(TableOfContentsNode currentNode, int fontSize, int verticalPosition, int pageNumber)
    {
        var sortedChildren = currentNode.Children.OrderBy(child => child.VerticalPosition);
        // Сначала проверяем детей текущего узла.
        foreach (var child in sortedChildren)
        {
            // Проверяем, соответствует ли дочерний узел условиям.
            if (child.FontSize > fontSize)
            {
                // Если размер шрифта у дочернего узла больше, проверяем страницу.
                if (child.PageNumber == pageNumber)
                {
                    // Если страница совпадает, проверяем положение на странице.
                    if (child.VerticalPosition > verticalPosition)
                    {
                        // Если положение на странице удовлетворяет условию, рекурсивно идем глубже.
                        return FindParentNode(child, fontSize, verticalPosition, pageNumber);
                    }
                }
            }
        }
        // Если не нашли подходящего дочернего узла, возвращаем текущий узел как родительский.
        return currentNode;        
    }

    private static List<TableOfContentsNode> getNextChilds(TableOfContentsNode parent, int fontSize, int pageNumber)
    {
        var matchingChilds = parent.Children.FindAll(child => child.FontSize > fontSize && pageNumber == child.PageNumber);
        return matchingChilds;
    }
}