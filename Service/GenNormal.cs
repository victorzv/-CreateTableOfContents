using TryPDFFile.Model;
using TryPDFFile.Service.Interface;

namespace TryPDFFile.Service;

public class GenNormal : ITableContentBuilder
{
    public Chapter build(List<ParagraphInfo> list)
    {
        Chapter root = new Chapter(int.MaxValue, -10, -10, "");

        var groupedByFontSize = list
            .OrderByDescending(element => element.FontSize)
            .GroupBy(element => element.FontSize)
            .ToDictionary(group => group.Key, group => group.ToList());

        double previousKey = groupedByFontSize.First().Key;
        var previousElements = groupedByFontSize.First().Value;

        foreach (var element in previousElements)
        {
            var newNode = new Chapter(element.FontSize, element.Page, element.AbsolutePosition, element.Text);
            root.AddChild(newNode);
        }

        foreach (var kvp in groupedByFontSize.Skip(1))
        {
            double fontSize = kvp.Key;
            List<ParagraphInfo> currentElements = kvp.Value;

            Chapter previousNode = null;

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
                var newNode = new Chapter(element.FontSize, element.Page, element.AbsolutePosition, element.Text);
                previousNode.AddChild(newNode);
            }

            previousElements = currentElements;
        }


        return root;
    }
}