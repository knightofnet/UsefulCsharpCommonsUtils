using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UsefulCsharpCommonsUtils.collection;
using UsefulCsharpCommonsUtils.lang;

namespace UsefulCsharpCommonsUtils.cli
{
    public class ConsoleArray<T>
    {

        public static void ConsoleWriteArray(T[] arrayElt, ConsoleArrayConfig config = null)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Type typeT = typeof(T);

            var props = typeT.GetProperties();
            if (!props.Any()) return;

            int cMax = Console.BufferWidth;
            int cMaxCorr = cMax - props.Length - 1;

            List<ConsoleArrayLine> lines = new List<ConsoleArrayLine>(1);

            List<ConsoleArrayColumn> cols = new List<ConsoleArrayColumn>(props.Length);

            ConsoleArrayLine lineHeader = new ConsoleArrayLine();
            for (int index = 0; index < props.Length; index++)
            {
                PropertyInfo prop = props[index];
                string prpName = prop.Name;
                int prpMaxWidth = (cMaxCorr) / props.Length;
                if (config != null)
                {
                    if (config.ColumnsName.IsIdxValid(index))
                    {
                        prpName = config.ColumnsName[index];
                    }

                    if (config.ColumnsMaxWidth.IsIdxValid(index))
                    {
                        prpMaxWidth = config.ColumnsMaxWidth[index];
                    }
                }

                ConsoleArrayColumn col = new ConsoleArrayColumn(prop.Name)
                {
                    Header = prpName,
                    MaxWidth = prpMaxWidth,
                    MaxContentWidth = prop.Name.Length
                };
                lineHeader.LineCols.Add(col, col.Header);

                cols.Add(col);
            }

            lines.Add(lineHeader);



            foreach (T elt in arrayElt)
            {
                ConsoleArrayLine line = new ConsoleArrayLine();
                foreach (ConsoleArrayColumn col in cols)
                {
                    object o = typeT.GetProperty(col.Name)?.GetValue(elt);

                    if (o != null)
                    {
                        line.AddCol(col, o.ToString());
                    }
                }

                lines.Add(line);
            }
            

            int sizeInterm = cols.Select(r => Math.Min(r.MaxContentWidth, r.MaxWidth)).Sum();
            bool hasDoneWork = true;
            while (sizeInterm < cMaxCorr && hasDoneWork)
            {
                hasDoneWork = false;
                foreach (ConsoleArrayColumn c in cols.Where(c => c.MaxContentWidth > c.MaxWidth))
                {
                    hasDoneWork = true;

                    c.MaxWidth++;
                    sizeInterm = cols.Select(r => Math.Min(r.MaxContentWidth, r.MaxWidth)).Sum();

                    if (sizeInterm == cMaxCorr)
                    {
                        break;
                    }

                }

                sizeInterm = cols.Select(r => Math.Min(r.MaxContentWidth, r.MaxWidth)).Sum();
            }

            for (int index = 0; index < lines.Count; index++)
            {
                ConsoleArrayLine line = lines[index];
                StringBuilder strB = new StringBuilder();

                int colIndex = 0;
                int colIndexCount = line.LineCols.Count;
                foreach (KeyValuePair<ConsoleArrayColumn, string> kvCol in line.LineCols)
                {
                    int effectiveWidth = Math.Min(kvCol.Key.MaxContentWidth, kvCol.Key.MaxWidth);
                    string val = CommonsStringUtils.Truncate(kvCol.Value, effectiveWidth);
                    if (val.Length != kvCol.Value.Length)
                    {
                        val = val.Substring(0, val.Length - 1) + "…";
                    }
                    strB.AppendFormat("{0, -" + effectiveWidth + "}", val);
                    if (colIndex < colIndexCount - 1)
                    {
                        strB.Append(" ");
                    }

                    colIndex++;
                }

                Console.WriteLine(strB.ToString());
                if (index == 0)
                {
                    Console.WriteLine(new string('-', strB.ToString().Trim().Length));
                }
            }
        }


        public class ConsoleArrayConfig
        {
            public string[] ColumnsName { get; set; }

            public int[] ColumnsMaxWidth { get; set; }
        }

        private class ConsoleArrayLine
        {
            public readonly Dictionary<ConsoleArrayColumn, string> LineCols = new Dictionary<ConsoleArrayColumn, string>();

            public void AddCol(ConsoleArrayColumn col, string toString)
            {
                LineCols.AddAndReplace(col, toString);
                if (col.MaxContentWidth < toString.Length)
                {
                    col.MaxContentWidth = toString.Length;
                }

                if (col.MinContentWidth > toString.Length)
                {
                    col.MinContentWidth = toString.Length;
                }
            }
        }

        private class ConsoleArrayColumn
        {

            public string Name { get; private set; }
            public string Header { get; set; }
            public int MaxWidth { get; set; }
            public int MaxContentWidth { get; set; }
            public int MinContentWidth { get; set; }
            public ConsoleArrayColumn(string propName)
            {
                Name = propName;
            }




        }
    }
}
