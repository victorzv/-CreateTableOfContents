using Aspose.Pdf.Annotations;
using Aspose.Pdf;
using TryPDFFile.Model;
using PdfOutliner.Model;

namespace PdfOutliner.Service
{
    public class OutlineBuilder
    {
        public void CreateOutlineHierarchy(string pdfFile, List<OutlineItem> o)
        {
            var pdfDoc = new Document(pdfFile);
            AddOutlinesToPdf(pdfDoc, o);
            pdfDoc.Save(pdfFile);
        }

        private void AddOutlinesToPdf(Document pdfDocument, List<OutlineItem> outlineItems)
        {
            foreach (var item in outlineItems)
            {
                AddOutlineItem(pdfDocument, pdfDocument.Outlines, null, item);
            }
        }

        private void AddOutlineItem(
            Document pdfDocument, 
            OutlineCollection outlines, 
            OutlineItemCollection parent, 
            OutlineItem item)
        {
            var outlineItemCollection = new OutlineItemCollection(outlines);
            outlineItemCollection.Title = item.Text;
            outlineItemCollection.Italic = true;
            outlineItemCollection.Action =
                new GoToAction(new XYZExplicitDestination(pdfDocument, item.Page, item.Left, item.Top, 1));

            if (parent != null)
                parent.Add(outlineItemCollection);
            else
                outlines.Add(outlineItemCollection);

            foreach (var child in item.Children)
            {
                AddOutlineItem(pdfDocument, outlines, outlineItemCollection, child);
            }
        }
    }
}
