namespace TryPDFFile;

public class LevelSetup
{
    public static List<ItemInfo> SetupLevel(List<ItemInfo> list)
    {
        var uniqueFontSizes = list.Select(item => item.FontSize).Distinct().OrderByDescending(size => size).ToList();

        Dictionary<double, int> fontSizeToLevel = new Dictionary<double, int>();
        int currentLevel = 0;

        foreach (var fontSize in uniqueFontSizes)
        {
            fontSizeToLevel[fontSize] = currentLevel;
            currentLevel++;
        }
        
        foreach (var item in list)
        {
            item.Level = fontSizeToLevel[item.FontSize];
        }

        return list;
    }
}