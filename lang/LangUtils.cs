using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang
{
    public static class LangUtils
    {

        public static bool IsEqWithNull<T>(T objA, T objB)
        {
            if (objA == null && objB != null) { return false; }
            if (objA != null && objB == null) { return false; }
            if (objA == null) { return false; }

            return objA.Equals(objB);
        }

    }
}
