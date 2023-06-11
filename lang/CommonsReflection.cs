using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang
{
    public static class CommonsReflection
    {

        public static PropertyInfo GetPropertyByNameComposed(Type type, string nomProp)
        {
            String[] splitted = nomProp.Split('.');

            Type lastType = type;
            PropertyInfo lastPropInfo = null;
            foreach (string nProp in splitted)
            {
                lastPropInfo = lastType.GetProperty(nProp);
                if (lastPropInfo == null) return null;

                lastType = lastPropInfo.PropertyType;
            }

            return lastPropInfo;
        }

        public static Object GetValueOfProp(Type type, string nomProp, Object obj)
        {
            String[] splitted = nomProp.Split('.');

            Type lastType = type;
            Object lastObj = obj;
            foreach (string nProp in splitted)
            {
                PropertyInfo lastPropInfo = lastType.GetProperty(nProp);
                if (lastPropInfo == null) return null;

                lastType = lastPropInfo.PropertyType;
                lastObj = lastPropInfo.GetValue(lastObj, null);
            }

            return lastObj;
        }

        public static void SetValueOfProp(Type type, string nomProp, Object obj, Object val)
        {
            String[] splitted = nomProp.Split('.');

            Type lastType = type;
            PropertyInfo lastPropInfo = null;
            Object lastObj = obj;
            foreach (string nProp in splitted)
            {
                lastPropInfo = lastType.GetProperty(nProp);
                if (lastPropInfo == null) break;

                lastType = lastPropInfo.PropertyType;
                lastObj = lastPropInfo.GetValue(lastObj, null);
            }

            lastPropInfo.SetValue(lastObj, val);
        }


        public static bool IsNullable(Type type)
        {
            return (object)Nullable.GetUnderlyingType(type) != null;
        }

    }
}
