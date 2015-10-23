using System;
using System.Collections.Generic;
using System.IO;

namespace ResumeProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = @"C:\Users\Thomas\Documents\Visual Studio 2015\Projects\ResumeProcessor\ResumeProcessor\data";

            var fm = new FileManager();
            //fm.SortFiles(basePath);

            string docPath = Path.Combine(basePath, "docx");

            string outputPath = Path.Combine(basePath, "txt");

            fm.ProcessFiles(docPath, outputPath);
        }


    }
}
