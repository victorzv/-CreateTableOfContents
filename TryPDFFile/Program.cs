using Aspose.Pdf;
using Aspose.Pdf.Text;
using TryPDFFile;

class Program
{
    static void main(string[] args)
    {
        Document pdfDoc = new Document("/home/tigra/sample.pdf");

        List<ItemInfo> itemsList = new List<ItemInfo>();

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
                ItemInfo itemInfo = new ItemInfo()
                {
                    FontName = textFragment.TextState.Font.FontName,
                    FontSize = textFragment.TextState.FontSize,
                    Page = page.Number,
                    Text = textFragment.Text,
                    VerticalPosition = textFragment.Position.YIndent,
                    HorisontalPosition = textFragment.Position.XIndent
                };

                itemsList.Add(itemInfo);
            }
        }

        IBuilderContent normalContent = new GenNormalContent();
        
        TableOfContentsNode content = normalContent.build(itemsList);
        
        content.Traverse(el=>Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition}"));

    }
}