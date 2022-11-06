using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang.ext
{
    public static class CommonsReflectionExt
    {

        public static PropertyInfo GetPropertyByNameComposed(this Type type, string nomProp)
        {
            return CommonsReflection.GetPropertyByNameComposed(type, nomProp);
        }

        public static Object GetValueOfProp(this Type type, string nomProp, Object obj)
        {
            return CommonsReflection.GetValueOfProp(type, nomProp, obj);
        }

        public static void SetValueOfProp(this Type type, string nomProp, Object obj, Object val)
        {
            CommonsReflection.SetValueOfProp(type, nomProp, obj, val);
        }
    }
}
