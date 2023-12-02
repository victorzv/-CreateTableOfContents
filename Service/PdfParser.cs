using Aspose.Pdf;
using Aspose.Pdf.Text;
using TryPDFFile.Model;
using TryPDFFile.Service.Interface;

namespace TryPDFFile.Service;

public class PdfParser : IPdfParser
{
    public List<ParagraphInfo> parseData(string pdfDocPath)
    {
        var pdfDoc = new Document(pdfDocPath);

        var itemsList = new List<ParagraphInfo>();
        var absorb = new ParagraphAbsorber();
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
                            if (textFragment.TextState.FontSize > maxFont)
                            {
                                maxFont = textFragment.TextState.FontSize;
                            }

                            if (textFragment.Page.Number > pageNumber)
                            {
                                pageNumber = textFragment.Page.Number;
                            }

                        }
                    }

                    var text = paragraph.Text;
                    
                    ParagraphInfo paragraphInfo = new ParagraphInfo()
                    {
                        FontSize = text.Length < 127 ? maxFont : 1,
                        Page = pageNumber,
                        Text = text,
                        AbsolutePosition = counter++,
                        Level = 0,
                        Top = paragraph.Fragments[0].Position.YIndent,
                        Left = paragraph.Fragments[0].Position.XIndent
                    };

                    itemsList.Add(paragraphInfo);
                }
            }
        }

        return itemsList;
    }
}