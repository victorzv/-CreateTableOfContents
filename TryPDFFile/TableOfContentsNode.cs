namespace TryPDFFile;

public class TableOfContentsNode
{
    public int FontSize { get; set; }
    public string Text { get; set; }
    public int VerticalPosition { get; set; }
    public int PageNumber { get; set; }
    public List<TableOfContentsNode> Children { get; set; }
    public int Depth { get; set; } // Добавляем уровень узла.

    public TableOfContentsNode(int fontSize, string text, int verticalPosition, int pageNumber, int depth = 0)
    {
        FontSize = fontSize;
        Text = text;
        VerticalPosition = verticalPosition;
        PageNumber = pageNumber;
        Children = new List<TableOfContentsNode>();
        Depth = depth; // Устанавливаем уровень узла.
    }

    public void AddChild(TableOfContentsNode child)
    {
        child.Depth = this.Depth + 1; // Устанавливаем уровень дочернего узла.
        Children.Add(child);
        //Children = Children.OrderByDescending(c => c.VerticalPosition).ToList();
        // Сначала сортируем дочерние узлы по странице по возрастанию.
        Children = Children.OrderBy(c => c.PageNumber).ThenByDescending(c=>c.VerticalPosition) .ToList();
    }

    // Реализация метода обхода дерева (пример обхода в глубину).
    public void Traverse(Action<TableOfContentsNode> action)
    {
        action(this);
        foreach (var child in Children)
        {
            child.Traverse(action);
        }
    }
}