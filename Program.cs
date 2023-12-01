using Aspose.Pdf;
using Aspose.Pdf.Annotations;
using System.Text.Json;
using TryPDFFile;

class Program
{
    static int Main(string[] args)
    {
        new License().SetLicense(@"./test.lic");
        List<IParagraphInfo> itemsList;

        IPdfParser pdfParser = new PdfParser();

        string pdfFileOutline;

        Console.WriteLine("Enter pdf file full path:");
        string pdfFile = args.Length == 0 ? Console.ReadLine() : args[0];
        if (!File.Exists(pdfFile))
        {
            Console.WriteLine($"File {pdfFile} doesn't exist");
            return -1;
        }

        string jsonFile = Path.ChangeExtension(pdfFile, "json");

        itemsList = pdfParser.parseData(pdfFile);
        string json = JsonSerializer.Serialize(itemsList, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFile, json);
        Console.WriteLine($"File {jsonFile} saved.");

        if (!File.Exists(jsonFile))
        {
            Console.WriteLine($"File {jsonFile} doesn't exist");
            return -1;
        }

        pdfFileOutline = Path.Combine(Path.GetDirectoryName(pdfFile), "tc_" + Path.GetFileName(pdfFile));

        List<IParagraphInfo> levelList = TableContentHierarchy.SetupLevel(itemsList);

        Document pdfDoc = new Document(pdfFileOutline);

        foreach (var item in levelList)
        {
            OutlineItemCollection outlineItemCollection = new OutlineItemCollection(pdfDoc.Outlines);
            outlineItemCollection.Title = item.Text;
            outlineItemCollection.Italic = true;
            outlineItemCollection.Action =
                new GoToAction(new XYZExplicitDestination(pdfDoc, item.Page, item.Left, item.Top, 1));

            pdfDoc.Outlines.Add(outlineItemCollection);

        }

        pdfDoc.Save(pdfFileOutline);
        foreach (var item in levelList)
        {
            Console.WriteLine($"{new string(' ', item.Level * 4)} FS({item.FontSize}) Page ({item.Page}) Text: {item.Text} AP {item.AbsolutePosition}");
        }

        return 0;
        /*
        IBuilderContent normalContent = new GenNormalContent();


          TableOfContentsNode content = normalContent.build(itemsList);


        content.Traverse(el =>
        {
            //if (el.Children.Any())
                Console.WriteLine($"{new string(' ', el.Depth * 4)}FS({el.FontSize}) Page {el.PageNumber} AP {el.AbsolutePosition} Text:\t{el.Text}");
        });
        */


    }
}