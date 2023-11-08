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
                    float maxFont = 0;
                    int pageNumber = 0;
                    foreach (List<TextFragment> line in paragraph.Lines)
                    {
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
                    }

                    int len = (paragraph.Text.Length > 10) ? 10 : paragraph.Text.Length;
                    var text = paragraph.Text;
                    text = text.Substring(1, len-1);
                    ItemInfo itemInfo = new ItemInfo()
                    {
                        FontSize = maxFont,
                        Page = pageNumber,
                        Text = text,
                        AbsolutePosition = counter++,
                        Level = 0
                    };

                    itemsList.Add(itemInfo);
                }
            }
        }

        return itemsList;
    }
}