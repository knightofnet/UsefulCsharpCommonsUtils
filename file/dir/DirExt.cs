using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.file.dir
{
    public static class DirExt
    {

        public static Dir ToDir(this DirectoryInfo dir)
        {
            return new Dir(dir.FullName);
        }

    }
}
