using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Monocle
{
    public class Files
    {
        public List<string> FileList { get; private set; } = new List<string>();

        public static string ExportPath { get; set; } = Environment.SpecialFolder.MyDocuments.ToString();

        public bool Add(string newFilePath)
        {
            if (System.IO.File.Exists(newFilePath) && Path.GetExtension(newFilePath).IsInputType() &&
                !FileList.Contains(newFilePath))
            {
                FileList.Add(newFilePath);
                return true;
            }
            return false;
        }

        public bool Remove(string RemoveFile)
        {
            if (FileList.First(b=>b == RemoveFile).Count() > 0)
            {
                FileList.Remove(RemoveFile);
                return true;
            }
            return false;
        }

        public bool Contains(string checkValue)
        {
            return FileList.Contains(checkValue);
        }
    }

    public static class FileExtensions
    {
        public static bool IsInputType(this string inputString)
        {
            inputString = inputString.ToLower().Replace(".","");
            return Enum.TryParse<InputFileType>(inputString, out InputFileType ift);
        }
    }

    public enum InputFileType
    {
        mzxml,
        mzml,
        raw,
        mgf
    }

    public enum OutputFileType
    {
        mzxml,
        csv
    }
}
