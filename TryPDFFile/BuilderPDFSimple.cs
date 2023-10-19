using System.Collections.Generic;
using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace TryPDFFile;

public class BuilderPDFSimple : IBuildData
{
    public List<ItemInfo> parseData(string pdfDocPath)
    {
        //new License().SetLicense("./test.lic");
        Document pdfDoc = new Document(pdfDocPath);

        List<ItemInfo> itemsList = new List<ItemInfo>();
        ParagraphAbsorber absorb = new ParagraphAbsorber();
        absorb.Visit(pdfDoc);
        int counter = 1;
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

                            if(textFragment.Page.Number > pageNumber)
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
                            Text = paragraph.Text,
                            AbsolutePosition = counter++
                        };

                        itemsList.Add(itemInfo);
                    }
                }
            }
        }

        return itemsList;
    }
}