using PdfOutliner.Service.Interface;
using TryPDFFile.Model;

namespace TryPDFFile.Service;

public class FontSizeFilter : ITableContentFilter
{
    public List<ParagraphInfo> Reduce(List<ParagraphInfo> list, int levels)
    {
        var uniqueFontSizes = list
                                .Select(item => item.FontSize)
                                .Distinct()
                                .OrderByDescending(size => size)
                                .Take(levels)
                                .ToList();

        var fontSizeToLevel = new Dictionary<double, int>();
        int currentLevel = 0;

        foreach (var fontSize in uniqueFontSizes)
        {
            fontSizeToLevel[fontSize] = currentLevel++;
        }

        var result= new List<ParagraphInfo>();
        foreach (var item in list)
        {
            if (fontSizeToLevel.ContainsKey(item.FontSize))
            {
                item.Level = fontSizeToLevel[item.FontSize];
                result.Add(item);
            }
        }

        return result;
    }
}