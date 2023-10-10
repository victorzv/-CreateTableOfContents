using Aspose.Pdf;
using Aspose.Pdf.Text;
using TryPDFFile;

class Program
{
    static void Main(string[] args)
    {
        //new License().SetLicense("./test.lic");
        Document pdfDoc = new Document("./test.pdf");

        List<ItemInfo> itemsList = new List<ItemInfo>();
        ParagraphAbsorber absorb = new ParagraphAbsorber();
        absorb.Visit(pdfDoc);

        foreach (PageMarkup markup in absorb.PageMarkups)
        {
            foreach (MarkupSection section in markup.Sections)
            {
                foreach (MarkupParagraph paragraph in section.Paragraphs)
                {
                    foreach (List<TextFragment> line in paragraph.Lines)
                    {
                        float maxFont = 0;
                        int pageNumber = 0;
                        foreach (TextFragment textFragment in line)
                        {
                            if(textFragment.TextState.FontSize > maxFont)
                            {
                                maxFont = textFragment.TextState.FontSize;                                
                            }

                            if(textFragment.Page.Number > maxFont)
                            {
                                pageNumber = textFragment.Page.Number;                                
                            }

                        }

                        //Console.WriteLine(pageNumber);
                        Console.WriteLine(paragraph.Text);
                        Console.WriteLine(maxFont);
                        Console.WriteLine("_____________________________");
                        ItemInfo itemInfo = new ItemInfo()
                        {
                            FontSize = maxFont,
                            Page = pageNumber,
                            Text = paragraph.Text
                        };

                        itemsList.Add(itemInfo);
                    }
                }
            }
        }

        IBuilderContent normalContent = new GenNormalContent();

        TableOfContentsNode content = normalContent.build(itemsList);

        content.Traverse(el =>
        {
            //if (el.Children.Any())
                Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition} Text:\t{el.Text}");
        });

    }
}