using Aspose.Pdf;
using Aspose.Pdf.Text;

class Program
{
    static void main(string[] args)
    {
        Document pdfDoc = new Document("/home/tigra/sample.pdf");

        List<FontInfo> fontInfoList = new List<FontInfo>();

        foreach (Page page in pdfDoc.Pages)
        {
            TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber();
            TextSearchOptions searchOptions = new TextSearchOptions(true);
            textFragmentAbsorber.TextSearchOptions = searchOptions;

            page.Accept(textFragmentAbsorber);
            
            TextFragmentCollection textFragments = textFragmentAbsorber.TextFragments;

            foreach (TextFragment textFragment in textFragments)
            {
                Console.WriteLine(textFragment.Page.Number);
                Console.WriteLine(textFragment.Position.YIndent);
                Console.WriteLine(textFragment.Text);
                Console.WriteLine(textFragment.TextState.FontSize);
                Console.WriteLine("_____________________________");
                FontInfo fontInfo = new FontInfo()
                {
                    FontName = textFragment.TextState.Font.FontName,
                    FontSize = textFragment.TextState.FontSize,
                    page = page.Number,
                    Text = textFragment.Text
                };
                
                fontInfoList.Add(fontInfo);
            }
        }

        foreach (FontInfo fontInfo in fontInfoList)
        {
            Console.WriteLine($"Page: {fontInfo.page},  Font: {fontInfo.FontName}, Size: {fontInfo.FontSize}");
        }
        
        fontInfoList = fontInfoList.OrderByDescending(f => f.FontSize).ToList();

        List<Node> nodes = new List<Node>();

        FontInfo rootNodeFontInfo = fontInfoList.FirstOrDefault();
        Node rootNode = new Node()
        {
            FontInfo = rootNodeFontInfo,
            Children = new List<Node>()
        };
        nodes.Add(rootNode);

        foreach (FontInfo fontInfo in fontInfoList.Skip(1))
        {
            Node childNode = new Node()
            {
                FontInfo = fontInfo,
                Children = new List<Node>()
            };

            Node parentNode = FindParentNode(rootNode, fontInfo);
            parentNode.Children.Add(childNode);
        }

        PrintNode(rootNode, "");
    }

    static Node FindParentNode(Node parentNode, FontInfo fontInfo)
    {
        foreach (Node childNode in parentNode.Children)
        {
            if (IsChildNode(fontInfo, childNode.FontInfo))
            {
                return FindParentNode(childNode, fontInfo);
            }
        }

        return parentNode;
    }

    static bool IsChildNode(FontInfo childFontInfo, FontInfo parentFontInfo)
    {
        double res = Math.Abs(childFontInfo.FontSize - parentFontInfo.FontSize); 
        return res < 1.0 && res != 0.0;
    }

    static void PrintNode(Node node, string indent)
    {
        Console.WriteLine($"{indent} Page: {node.FontInfo.page}, Font: {node.FontInfo.FontName}, Size: {node.FontInfo.FontSize}, Text: {node.FontInfo.Text}");
        foreach (Node childNode in node.Children)
        {
            PrintNode(childNode, indent + " ");
        }
    }
}

class FontInfo
{
    public string FontName { get; set; }
    public double FontSize { get; set; }

    public int page { get; set; }

    public string Text { get; set; }
    
}

// Класс для представления узла в древовидной структуре
class Node
{
    public FontInfo FontInfo { get; set; }
    public List<Node> Children { get; set; }
}