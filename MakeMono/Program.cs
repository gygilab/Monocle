using Monocle;
using System;
using System.IO;

namespace MakeMono
{
    class Program
    {
        static void Main(string[] args)
        {
            FileProcessor Processor = new FileProcessor();

            if (args.Length < 1)
            {
                Console.WriteLine("MakeMono, a console application wrapper for Monocle.");
                Console.WriteLine("The first argument should be a valid input type (e.g. mzXML)");
                Console.WriteLine("Example: MakeMono.exe 'C:\\MY_FILE.mzXML'");
                return;
            }

            bool process = false;
            string filePath = args[0];
            Console.WriteLine("Starting Monocle on: " + filePath);
            if (File.Exists(filePath))
            {
                process = true;
            }

            if (process)
            {
                if (Processor.files.Add(filePath))
                {
                    Files.ExportPath = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), "");
                }
                else
                {
                    Console.WriteLine("The input file is not an acceptable type:");
                    Console.WriteLine("the file extension should be: .mzXML or .RAW");
                    return;
                }

                Processor.Run(true);
            }
            else
            {
                Console.WriteLine("No file exists at the given path.");
            }
        }

        public void FileTracker_Listener(object sender, FileEventHandler e)
        {

        }

    }
}
