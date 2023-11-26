namespace TryPDFFile;

public class Chapter
{
    public double FontSize { get; set; }
    public int PageNumber { get; set; }

    public string Text { get; set; }
    public int AbsolutePosition { get; set; }
    public List<Chapter> Children { get; set; }
    public int Depth { get; set; } // Добавляем уровень узла.

    public Chapter(double fontSize, int pageNumber, int absolutePosition, string text, int depth = 0)
    {
        FontSize = fontSize;
        PageNumber = pageNumber;
        AbsolutePosition = absolutePosition;
        Children = new List<Chapter>();
        Text = text;
        Depth = depth; // Устанавливаем уровень узла.
    }

    public void AddChild(Chapter child)
    {
        child.Depth = this.Depth + 1; // Устанавливаем уровень дочернего узла.
        //Console.WriteLine($"ADD child to ({FontSize}, {PageNumber}, {VerticalPosition}, {AbsolutePosition}, {Depth})  child ({child.FontSize}, {child.PageNumber}, {child.VerticalPosition}, {child.AbsolutePosition}, {child.Depth}");
        Children.Add(child);
        Children = Children.OrderBy(c=>c.AbsolutePosition).ToList();
    }

    // Реализация метода обхода дерева (пример обхода в глубину).
    public void Traverse(Action<Chapter> action)
    {
        action(this);
        foreach (var child in Children)
        {
            child.Traverse(action);
        }
    }
    
    public Chapter FindNode(int absolutePosition)
    {
        if (this.AbsolutePosition == absolutePosition)
        {
            return this;
        }

        foreach (var child in Children)
        {
            var foundNode = child.FindNode(absolutePosition);
            if (foundNode != null)
            {
                return foundNode;
            }
        }

        return null;
    }
}