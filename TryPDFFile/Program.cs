﻿using System;
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
        new License().SetLicense(@"./test.lic");
        List<ItemInfo> itemsList = null;
        
        IBuildData buildData = new BuilderPDFSimple();

        String pdfFileOutline = "";
        string input = Console.ReadLine();

        if (int.TryParse(input, out int choice))
        {
            Console.WriteLine("Enter pdf file full path:");
            string pdfFile = args.Length == 0 ? Console.ReadLine() : args[0];
            string jsonFile = Path.ChangeExtension(pdfFile, "json");
            
            if (!File.Exists(pdfFile))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            
            itemsList = buildData.parseData(pdfFile);
            string json = JsonConvert.SerializeObject(itemsList, Formatting.Indented);
            File.WriteAllText(jsonFile, json);
            Console.WriteLine($"File {jsonFile} saved.");

            if (!File.Exists(jsonFile))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            
            pdfFileOutline = pdfFile;
            itemsList = buildData.parseData(pdfFile);
        }

        List<ItemInfo> levelList = LevelSetup.SetupLevel(itemsList);

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