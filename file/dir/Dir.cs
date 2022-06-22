using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UsefulCsharpCommonsUtils.file.dir
{
    public class Dir
    {

        //private static readonly NLog.Logger _log_ = NLog.LogManager.GetCurrentClassLogger();

        private DirectoryInfo _innerDir;

        public string Fullname => _innerDir?.FullName;
        public string Name => _innerDir?.Name;

        public bool Exists => _innerDir != null && _innerDir.Exists;



        public Dir this[string s]
        {
            get => Child(s);
        }

        public FileInfo this[string s, int mode]
        {
            get => ChildFile(s, mode);
        }



        public Dir(string path=".")
        {
            //_log_.Debug($"NewDir:{path}");
            _innerDir = new DirectoryInfo(path);

        }

        public void Delete()
        {
            DeleteDirectory(_innerDir.FullName);
        }

        public void ClearContent()
        {
            foreach (FileInfo file in _innerDir.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in _innerDir.EnumerateDirectories())
            {
                DeleteDirectory(dir.FullName);
            }
        }

        public bool IsExists()
        {
            return _innerDir.Exists;
        }


        public FileInfo GetFileThatMatch(string fileMask)
        {
            //_log_.Debug($"GetFileThatMatch:{fileMask}, From:{_innnerDir.FullName}");
            _innerDir.Refresh();
            return _innerDir.EnumerateFiles(fileMask).FirstOrDefault();
        }

        public FileInfo[] GetFilesThatMatch(string fileMask)
        {
            //_log_.Debug($"GetFilesThatMatch:{fileMask}, From:{_innnerDir.FullName}");
            _innerDir.Refresh();
            return _innerDir.GetFiles(fileMask);
        }

        public Dir GetDirThatMatch(string fileMask)
        {
            //_log_.Debug($"GetDirThatMatch:{fileMask}, From:{_innnerDir.FullName}");
            _innerDir.Refresh();
            return new Dir(_innerDir.EnumerateDirectories(fileMask).FirstOrDefault()?.FullName);
        }



        public Dir[] GetDirsThatMatch(string fileMask)
        {
            //_log_.Debug($"GetDirsThatMatch:{fileMask}, From:{_innnerDir.FullName}");
            _innerDir.Refresh();
            DirectoryInfo[] dirs = _innerDir.GetDirectories(fileMask);
            if (!dirs.Any()) return new Dir[] { };

            return dirs.Select(r => new Dir(r.FullName)).ToArray();
        }

        public string CombineWithFile(string fileName)
        {
            return Path.Combine(_innerDir.FullName, fileName);
        }

        public void CreateIfNot()
        {
            if (IsExists()) return;

            Create();
            _innerDir.Refresh();
        }

        public Dir[] ChildDirs()
        {
            return _innerDir?.EnumerateDirectories().Select(directoryInfo => new Dir(directoryInfo.FullName)).ToArray();
        }

        public FileInfo[] ChildFiles()
        {
            return _innerDir?.EnumerateFiles().ToArray();
        }



        public FileSystemInfo[] Children(bool isRecurse = true)
        {
            List<FileSystemInfo> retList = new List<FileSystemInfo>();

            foreach (FileInfo file in _innerDir.EnumerateFiles())
            {
                retList.Add(file);
            }

            foreach (DirectoryInfo dir in _innerDir.EnumerateDirectories())
            {
                retList.Add(dir);
                Dir d = new Dir(dir.FullName);
                if (isRecurse)
                {
                    retList.AddRange(d.Children());
                }
            }

            return retList.ToArray();
        }

        public Dir CreateChildIfNot(string childRelative)
        {
            string[] recDir = childRelative.Split('\\');

            Dir currentdir = this;

            foreach (string recPath in recDir)
            {
                currentdir = new Dir(Path.Combine(currentdir.Fullname, recPath));
                if (!currentdir.Exists)
                {
                    currentdir.CreateIfNot();
                }
            }

            return currentdir;
        }

        public bool IsExistsDirChild(string child)
        {
            return Directory.EnumerateFiles(Path.Combine(_innerDir.FullName, child)).Any();
        }

        public DirectoryInfo DirectoryInfo()
        {
            return _innerDir;
        }

        private void Create()
        {
            _innerDir.Create();
        }

        private Dir Child(string s)
        {
            //_log_.Debug($"Child:{s}, From:{_innnerDir.FullName}");
            try
            {
                _innerDir.Refresh();
                return new Dir(_innerDir.EnumerateDirectories().FirstOrDefault(r => r.Name.Equals(s))?.FullName);
            }
            catch (Exception e)
            {
                //_log_.Error(e);
                //_log_.Error(_innnerDir.EnumerateDirectories().Select(r => r.FullName).Aggregate((a, b) => $"{a},{b}"));
                throw e;
            }
        }

        private FileInfo ChildFile(string s, int mode)
        {
            return GetFileThatMatch(s);
        }

        private void DeleteDirectory(string d)
        {
            foreach (var sub in Directory.EnumerateDirectories(d))
            {
                DeleteDirectory(sub);
            }
            foreach (var f in Directory.EnumerateFiles(d))
            {
                var fi = new FileInfo(f);
                fi.Attributes = FileAttributes.Normal;
                fi.Delete();
            }
            Directory.Delete(d);
        }




        public static FileSystemInfo[] Children(DirectoryInfo directory, bool isRecurse = true)
        {
            return Children(directory.FullName, isRecurse);
        }

        public static FileSystemInfo[] Children(string directoryPath, bool isRecurse = true)
        {
            Dir d = new Dir(directoryPath);
            return d.Children(isRecurse);
        }

    }
}
