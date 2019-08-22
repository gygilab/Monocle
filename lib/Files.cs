using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonocleUI
{
    public class Files
    {
        public List<string> FileList { get; private set; } = new List<string>();

        public bool Add(string newFilePath)
        {
            if (File.Exists(newFilePath) && Path.GetExtension(newFilePath).IsInputType() &&
                !FileList.Contains(newFilePath))
            {
                FileList.Add(newFilePath);
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
}
