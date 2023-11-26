namespace TryPDFFile;

public class IParagraphInfo
{
    public double FontSize { get; set; }
    
    public int Page { get; set; }
    
    public string Text { get; set; }

    public double Top { get; set; }
    
    public double Left { get; set; }
    
    public int AbsolutePosition { get; set; }

    public int Level { get; set; }
}