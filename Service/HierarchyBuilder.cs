using PdfOutliner.Model;
using TryPDFFile.Model;

namespace PdfOutliner.Service
{
    public class HierarchyBuilder
    {
        public List<OutlineItem> Build(List<ParagraphInfo> items)
        {
            List<OutlineItem> outline = new List<OutlineItem>();
            Stack<OutlineItem> stack = new Stack<OutlineItem>();

            foreach (var item in items)
            {
                while (stack.Count > 0 && stack.Peek().Level >= item.Level)
                {
                    stack.Pop();
                }

                OutlineItem parent = (stack.Count > 0) ? stack.Peek() : null;
                var o = new OutlineItem
                {
                    Left = item.Left,
                    AbsolutePosition = item.AbsolutePosition,
                    Level = item.Level,
                    Page = item.Page,
                    Text = item.Text,
                    Top = item.Top
                };
                if (parent != null)
                {
                    parent.Children.Add(o);
                }
                else
                {
                    outline.Add(o);
                }

                stack.Push(o);
            }

            return outline;
        }
    }
}
