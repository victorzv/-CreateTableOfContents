using Aspose.Pdf;
using PdfOutliner.Model;
using PdfOutliner.Service;
using System.Text.Json;
using TryPDFFile.Model;
using TryPDFFile.Service;
using TryPDFFile.Service.Interface;

class Program
{
    static int Main(string[] args)
    {
        new License().SetLicense(@"./test.lic");
        List<ParagraphInfo> itemsList;

        IPdfParser pdfParser = new PdfParser();

        string pdfFileOutline;

        Console.WriteLine("Enter pdf file full path:");
        string pdfFile = args.Length == 0 ? Console.ReadLine() : args[0];
        Console.WriteLine("Levels:");
        int levels = args.Length == 0 ? int.Parse(Console.ReadLine()) : int.Parse(args[1]);
        if (!File.Exists(pdfFile))
        {
            Console.WriteLine($"File {pdfFile} doesn't exist");
            return -1;
        }

        string jsonFile = Path.ChangeExtension(pdfFile, "json");

        itemsList = pdfParser.parseData(pdfFile);
        
        pdfFileOutline = Path.Combine(Path.GetDirectoryName(pdfFile), "tc_" + Path.GetFileName(pdfFile));
        File.Copy(pdfFile, pdfFileOutline);

        List<ParagraphInfo> levelList = new FontSizeFilter().Reduce(itemsList, levels);

        
        List<OutlineItem> hierarchy = new HierarchyBuilder().Build(levelList);
        string json = JsonSerializer.Serialize(hierarchy, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFile, json);
        Console.WriteLine($"File {jsonFile} saved.");

        new OutlineBuilder().CreateOutlineHierarchy(pdfFileOutline, hierarchy);
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