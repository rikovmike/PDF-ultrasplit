using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace PDF_ultrasplit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Rikovmike PDF splitter");

            if (args.Length > 0)
            {
                Console.WriteLine(args[0]);

                FileAttributes attr = File.GetAttributes(args[0]);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string[] files = Directory.GetFiles(args[0]);
                    foreach (string file in files)
                        if (Path.GetExtension(file).ToLower() == ".pdf")
                        {
                            splitfile(Path.Combine(args[0], Path.GetFileName(file)));
                        }

                }

                else
                {
                    if (Path.GetExtension(args[0]).ToLower() == ".pdf")
                    {
                        splitfile(args[0]);
                    }
                }
                    

                    
            }
            
        }





        static void splitfile(string filename)
        {

            Console.WriteLine($"-- {Path.GetFileName(filename)}");
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Open the file
            PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import);



            string name = Path.GetFileNameWithoutExtension(filename);
            string exportdir = Path.Combine(Path.GetDirectoryName(filename), name);



            if (!Directory.Exists(exportdir))
            {
                Directory.CreateDirectory(exportdir);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(exportdir);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }

            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create new document
                PdfDocument outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title =
                  String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[idx]);
                outputDocument.Save(Path.Combine(exportdir,String.Format("{0}-{1}.pdf", name, idx + 1)));
            }

        }


    }
}
