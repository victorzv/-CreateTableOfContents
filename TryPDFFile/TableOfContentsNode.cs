using System;
using System.Collections.Generic;
using System.Linq;

namespace TryPDFFile;

public class TableOfContentsNode
{
    public double FontSize { get; set; }
    public int PageNumber { get; set; }

    public string Text { get; set; }
    public int AbsolutePosition { get; set; }
    public List<TableOfContentsNode> Children { get; set; }
    public int Depth { get; set; } // Добавляем уровень узла.

    public TableOfContentsNode(double fontSize, int pageNumber, int absolutePosition, string text, int depth = 0)
    {
        FontSize = fontSize;
        PageNumber = pageNumber;
        AbsolutePosition = absolutePosition;
        Children = new List<TableOfContentsNode>();
        Text = text;
        Depth = depth; // Устанавливаем уровень узла.
    }

    public void AddChild(TableOfContentsNode child)
    {
        child.Depth = this.Depth + 1; // Устанавливаем уровень дочернего узла.
        //Console.WriteLine($"ADD child to ({FontSize}, {PageNumber}, {VerticalPosition}, {AbsolutePosition}, {Depth})  child ({child.FontSize}, {child.PageNumber}, {child.VerticalPosition}, {child.AbsolutePosition}, {child.Depth}");
        Children.Add(child);
        Children = Children.OrderBy(c=>c.AbsolutePosition).ToList();
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
    
    public TableOfContentsNode FindNode(int absolutePosition)
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