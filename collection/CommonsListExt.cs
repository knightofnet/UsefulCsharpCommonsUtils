using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.collection
{
    public static class CommonsListExt
    {

        public static bool IsIdxValid<T>(this IList<T> lst, int idx)
        {
            if (lst == null || !lst.Any()) return false;

            if (idx > lst.Count - 1) return false;

            return true;
        }

    }
}
