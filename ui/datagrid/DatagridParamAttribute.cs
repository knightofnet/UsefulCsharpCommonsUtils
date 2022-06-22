using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UsefulCsharpCommonsUtils.ui.datagrid
{
    public class DatagridParamAttribute : Attribute
    {
        public string DatagridHeader { get; }
        public bool IstoShow { get; }
        public DataGridLength ColWidth { get; }
        public bool IsReadOnly { get; set; }

        public DatagridParamAttribute(string datagridHeader = null, bool isToShow = true, string colWidth = "1,auto", bool isReadOnly = false)
        {
            DatagridHeader = datagridHeader;
            IstoShow = isToShow;

            ColWidth = Parse(colWidth);
            IsReadOnly = isReadOnly;
        }

        private DataGridLength Parse(string colWidthStr)
        {

            string[] splitted = colWidthStr.Split(',');
            if (splitted.Length >= 1 && double.TryParse(splitted[0].Trim(), out double sizeLoc))
            {
                if (splitted.Length == 1) return new DataGridLength(sizeLoc);

                if (splitted.Length == 2 && Enum.TryParse(splitted[1].Trim(), true, out DataGridLengthUnitType unit))
                {
                    return new DataGridLength(sizeLoc, unit);
                }


            }

            return new DataGridLength(1, DataGridLengthUnitType.Auto);
        }
    }
}