
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UsefulCsharpCommonsUtils.collection;

namespace UsefulCsharpCommonsUtils.ui.datagrid
{
    public static class AnnotationUtils
    {
        //private static readonly NLog.Logger _log_ = NLog.LogManager.GetCurrentClassLogger();


        private static Dictionary<string, string> propNameByColName = new Dictionary<string, string>();


        public static string GetFirstPropnameFromColumnName<T>(string columnName)
        {
            //_log_.Debug($"GetFirstPropnameFromColumnName(columnName: {columnName})");

            Type type = typeof(T);
            string key = $"{type.FullName}-{columnName}";

            if (propNameByColName.ContainsKey(key))
            {
                //_log_.Debug($"GetFirstPropnameFromColumnName(columnName: {columnName}) => CACHE {propNameByColName[key]}");
                return propNameByColName[key];
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                string locColumnName = GetDgColumnName<T>(property.Name);
                string lKey = $"{type.FullName}-{locColumnName}";
                propNameByColName.AddNew(lKey, property.Name);
                if (locColumnName.Equals(columnName))
                {
                    //_log_.Debug($"GetFirstPropnameFromColumnName(columnName: {columnName}) => {property.Name}");
                    return property.Name;
                }
            }

            return null;
        }

        public static string GetDgColumnName<T>(string propName)
        {
            DatagridParamAttribute dgPa = GetDatagridParamAttribute<T>(propName);

            if (dgPa != null)
            {
                return dgPa.DatagridHeader;
            }

            return propName;

        }

        public static DataGridLength GetDgColumnWidth<T>(string propName)
        {
            DatagridParamAttribute dgPa = GetDatagridParamAttribute<T>(propName);

            if (dgPa != null)
            {
                return dgPa.ColWidth;
            }

            return double.NaN;

        }

        public static bool IsReadonlyDgColumn<T>(string propName, bool defValue = false)
        {
            DatagridParamAttribute dgPa = GetDatagridParamAttribute<T>(propName);

            if (dgPa != null)
            {
                return dgPa.IsReadOnly;
            }

            return defValue;
        }

        public static bool IstoShowDgColumn<T>(string propName, bool defValue = true)
        {
            DatagridParamAttribute dgPa = GetDatagridParamAttribute<T>(propName);

            if (dgPa != null)
            {
                return dgPa.IstoShow;
            }

            return defValue;
        }

        public static DatagridParamAttribute GetDatagridParamAttribute<T>(string propName)
        {
            Type typeDoIt = typeof(T);

            return (DatagridParamAttribute)typeDoIt.GetProperty(propName).GetCustomAttributes(typeof(DatagridParamAttribute), false)?.FirstOrDefault();
        }




    }
}
