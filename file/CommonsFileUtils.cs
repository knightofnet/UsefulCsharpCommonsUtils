using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.file
{
    public static class CommonsFileUtils
    {

        private static readonly string[] Sizes = { "o", "ko", "Mo", "Go", "To" };

        public static FileInfo Rename(this FileInfo file, string newName, bool throwExceptionIfFail = true)
        {
            if (!file.Exists || newName == null) return null;

            try
            {
                FileInfo newfile = file.CopyTo(Path.Combine(file.Directory.FullName, newName));
                newfile.Attributes = file.Attributes;
                file.Attributes = FileAttributes.Normal;
                file.Delete();

                return newfile;
            }
            catch (Exception ex)
            {
                if (throwExceptionIfFail) throw ex;
                return null;
            }
        }


        public static string SuffixedFileName(FileInfo file, string suffix)
        {
            if (file == null) return suffix;
            if (string.IsNullOrEmpty(suffix)) return file.Name;

            string s = file.Name.Replace(file.Extension, string.Empty);
            s += suffix;
            return s + file.Extension;
        }


        public static string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public static void OpenFolderInWindowsExplorer(string dirItemFullName)
        {
            OpenFolderInWindowsExplorer(new DirectoryInfo(dirItemFullName));
        }

        public static void OpenFolderInWindowsExplorer(DirectoryInfo directory)
        {
            Process.Start("explorer.exe", directory.FullName);
        }

        public static void ShowFileInWindowsExplorer(string stringPath)
        {
            FileInfo fi = new FileInfo(stringPath);
            ShowFileInWindowsExplorer(fi);
        }

        public static void ShowFileInWindowsExplorer(FileSystemInfo file)
        {
            Process.Start("explorer.exe", "/select, \"" + file.FullName + "\"");
        }

        public static void OpenWithSystemShell(FileInfo file)
        {
            OpenWithSystemShell(file.FullName);
        }
        public static void OpenWithSystemShell(string path )
        {
            Process.Start("explorer.exe", "\"" + path + "\"");
        }

        public static string HumanReadableSize(long size, string format = "{0:0.##} {1}")
        {
            double len = size;
            int order = 0;
            while (len >= 1024 && order < Sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return $"{len:0.##} {Sizes[order]}";

        }

        public static double HumanReadableSizeToLong(string hrSize)
        {
            return HumanReadableSizeToLong(hrSize, false);

        }

        public static double HumanReadableSizeToLong(string hrSize, bool isCaseSensitiveUnit)
        {
            double retDouble = -1;

            string sizes = string.Join("|", Sizes);
            Regex rgx = new Regex(@"(\d.*?(|,\d.*?))\s{0,1}(" + sizes + ")", RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(hrSize);
            if (matches.Count == 1)
            {
                Match match = matches[0];
                string size = match.Groups[1] + match.Groups[2].ToString();
                string unit = match.Groups[3].ToString();

                int i = 0;
                foreach (string unitTab in Sizes)
                {

                    if (unit == unitTab || (!isCaseSensitiveUnit && unit.ToUpper().Equals(unitTab.ToUpper())))
                    {
                        break;
                    }
                    i++;
                }

                double dblSize = 0;
                if (double.TryParse(size, out dblSize))
                {
                    retDouble = dblSize * Math.Pow(2, 10 * i);
                }





            }

            return retDouble;

        }



    }
}
