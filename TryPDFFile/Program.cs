using System;
using System.Collections.Generic;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using TryPDFFile;

class Program
{
    static void Main(string[] args)
    {

        IBuildData buildData = new BuilderPDFSimple();

        List<ItemInfo> itemsList = buildData.parseData("./test.pdf");

        IBuilderContent normalContent = new GenNormalContent();

        TableOfContentsNode content = normalContent.build(itemsList);

        content.Traverse(el =>
        {
            //if (el.Children.Any())
                Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition} Text:\t{el.Text}");
        });

    }
}