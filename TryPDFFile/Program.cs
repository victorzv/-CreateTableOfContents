using System;
using System.Collections.Generic;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using TryPDFFile;

class Program
{
    static void Main(string[] args)
    {

        List<ItemInfo> itemsList = new List<ItemInfo>();

        IBuilderContent normalContent = new GenNormalContent();

        TableOfContentsNode content = normalContent.build(itemsList);

        content.Traverse(el =>
        {
            //if (el.Children.Any())
                Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition} Text:\t{el.Text}");
        });

    }
}