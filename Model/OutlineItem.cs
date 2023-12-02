namespace PdfOutliner.Model
{
    public class OutlineItem
    {
        public string Text { get; set; }

        public int Page { get; set; }

        public int Level { get; set; }

        public double Top { get; set; }

        public double Left { get; set; }

        public int AbsolutePosition { get; set; }

        public List<OutlineItem> Children { get; set; } = new List<OutlineItem>();
    }
}
