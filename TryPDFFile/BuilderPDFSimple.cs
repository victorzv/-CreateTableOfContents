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

                    //int len = (paragraph.Fragments[0].Text.Length > 30) ? 30 : paragraph.Fragments[0].Text.Length;
                    
                    var text = paragraph.Fragments[0].Text;
                    /*
                    if (len > 1)
                    {
                        len--;
                    }*/

                    //text = text.Substring(0, len);
                    ItemInfo itemInfo = new ItemInfo()
                    {
                        FontSize = maxFont,
                        Page = pageNumber,
                        Text = text,
                        AbsolutePosition = counter++,
                        Level = 0,
                        Top = paragraph.Fragments[0].Position.YIndent,
                        Left = paragraph.Fragments[0].Position.XIndent
                    };

                    itemsList.Add(itemInfo);
                }
            }
        }

        return itemsList;
    }
}