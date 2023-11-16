using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Pdf;
using Aspose.Pdf.Annotations;
using Aspose.Pdf.Text;
using Newtonsoft.Json;
using TryPDFFile;

class Program
{
    static void Main(string[] args)
    {
        new License().SetLicense("/home/tigra/test.lic");
        List<ItemInfo> itemsList = null;
        
        IBuildData buildData = new BuilderPDFSimple();

        String pdfFileOutline = "";
        
        Console.WriteLine("Enter 1 to create json file from pdf file");
        Console.WriteLine("Enter 2 to read from json file and run program");
        Console.WriteLine("Enter 3 to read from pdf file and run program");
        Console.WriteLine("Enter 4 to read from preset pdf file and run program");

        string input = Console.ReadLine();

        if (int.TryParse(input, out int choice))
        {
            if (choice == 1)
            {
                Console.WriteLine("Enter pdf file full path:");
                string pdfFile = Console.ReadLine();
                if (!File.Exists(pdfFile))
                {
                    Console.WriteLine("File doesn't exist");
                    return;
                }
                else
                {
                    itemsList = buildData.parseData(pdfFile);
                    Console.WriteLine("Enter json file full path to save:");
                    string jsonFile = Console.ReadLine();
                    string json = JsonConvert.SerializeObject(itemsList, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                    Console.WriteLine($"File {jsonFile} saved.");
                    return;
                }
            }

            if (choice == 2)
            {
                Console.WriteLine("Enter json file name full path:");
                string jsonFile = Console.ReadLine();
                if (!File.Exists(jsonFile))
                {
                    Console.WriteLine("File doesn't exist");
                    return;
                }
                else
                {
                    string json = File.ReadAllText(jsonFile);
                    itemsList = JsonConvert.DeserializeObject<List<ItemInfo>>(json);
                }
            }

            if (choice == 3)
            {
                Console.WriteLine("Enter pdf file full path:");
                string pdfFile = Console.ReadLine();
                pdfFileOutline = pdfFile;
                itemsList = buildData.parseData(pdfFile);        
            }

            if (choice == 4)
            {
                itemsList = buildData.parseData("./test.pdf");       
            }
        }

        List<ItemInfo> levelList = LevelSetup.SetupLevel(itemsList);

        if (choice == 3)
        {
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
        }
        
        
        foreach(var item in levelList)
        {
            Console.WriteLine($"{new string(' ', item.Level * 4)} FS({item.FontSize}) Page ({item.Page}) Text: {item.Text} AP {item.AbsolutePosition}");
        }

        
        
        
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