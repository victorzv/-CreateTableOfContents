using TryPDFFile.Model;

namespace TryPDFFile.Service;

public class TableContentHierarchy
{
    public static List<ParagraphInfo> SetupLevel(List<ParagraphInfo> list, int levels)
    {
        var uniqueFontSizes = list.Select(item => item.FontSize).Distinct().OrderByDescending(size => size).Take(levels).ToList();

        Dictionary<double, int> fontSizeToLevel = new Dictionary<double, int>();
        int currentLevel = 0;

        foreach (var fontSize in uniqueFontSizes)
        {
            fontSizeToLevel[fontSize] = currentLevel++;
        }
        List < ParagraphInfo > result= new List<ParagraphInfo>();
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