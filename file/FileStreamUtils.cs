using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.file
{
    public static class FileStreamUtils
    {

        public static StreamWriter Utf8NoBomStreamWriter(string filePath, FileMode filemode)
        {
            return new StreamWriter(File.Open(filePath, filemode), new UTF8Encoding(false));
        }

        public static StreamWriter Utf8BomStreamWriter(string filePath, FileMode filemode)
        {
            return new StreamWriter(File.Open(filePath, filemode), new UTF8Encoding(true));
        }

    }
}
