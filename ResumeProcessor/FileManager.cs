using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ResumeProcessor
{
    class FileManager
    {
        public List<ResumeFile> SortFiles(string directoryPath)
        {
            var resumeList = new List<ResumeFile>();
            var files = Directory.GetFiles(directoryPath);
            foreach (string f in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(f);
                var extension = Path.GetExtension(f).ToLower();
                var extensionDir = Path.Combine(directoryPath, extension.TrimStart('.'));
                if (!Directory.Exists(extensionDir)) {
                    // Create the directory like "pdf", "doc" or "docx"
                    Directory.CreateDirectory(extensionDir);
                }
                // Move the file to the approapriate directory
                File.Move(f, Path.Combine(extensionDir, Path.GetFileName(f)));

                resumeList.Add(new ResumeFile { FileName = fileName, FileType = extension });
            }
            return resumeList;
        }

        public void ProcessFiles(string directoryPath, string outputPath, bool prependFileName = true)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            var files = Directory.GetFiles(directoryPath);
            foreach (string f in files)
            {
                string textFileName = Path.GetFileNameWithoutExtension(f) + ".txt";

                DocxToText dtt = new DocxToText(f);
                string textContent = dtt.ExtractText();

                string outputFile = Path.Combine(outputPath, textFileName);
                if (!File.Exists(outputFile))
                {
                    string createText = Regex.Replace(textContent + Environment.NewLine, @"\t|\n|\r", " ");
                    if (prependFileName)
                        createText = Path.GetFileNameWithoutExtension(f) + "," + FilterWhiteSpaces(createText);
                    else
                        createText = FilterWhiteSpaces(createText);
                    File.WriteAllText(outputFile, createText);
                }
            }
        }

        public string FilterWhiteSpaces(string input)
        {
            if (input == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (i == 0 || c != ' ' || (c == ' ' && input[i - 1] != ' '))
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }
}
