using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleUI.lib
{
    class FileProcessor : IDisposable
    {
        StreamReader Reader;
        FileWriter Writer;

        public Files files { get; set; } = new Files();

        /// <summary>
        /// Listener pair to track processing progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void FileEventHandler(Object sender, FileEventArgs e);
        public class FileEventArgs : EventArgs
        {
            public string FilePath;
            public FileEventArgs(string filePath) { FilePath = filePath; }
        }

        public FileProcessor()
        {
            Writer = new FileWriter();
        }

        /// <summary>
        /// Processes a single file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="inputFileType"></param>
        public void Run(string filePath, InputFileType inputFileType)
        {
            Reader = new StreamReader(filePath);
        }

        /// <summary>
        /// Process a batch of files.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="inputFileType"></param>
        public void Run(string[] filePaths, InputFileType inputFileType)
        {
            foreach(string filePath in filePaths)
            {
                using (Reader = new StreamReader(filePath))
                {

                }
            }
        }

        public void Dispose()
        {
            Reader.Dispose();
        }
    }
}
