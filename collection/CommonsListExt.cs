using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.collection
{
    /// <summary>
    /// CommonsListExt
    /// </summary>
    public static class CommonsListExt
    {

        public static bool IsIdxValid<T>(this IList<T> lst, int idx)
        {
            if (lst == null || !lst.Any()) return false;

            if (idx > lst.Count - 1) return false;

            return true;
        }

        /// <summary>
        /// Update a list from another. Return a boolean which indicate if an update has been doned.
        /// Update based on hash.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="otherList"></param>
        /// <returns></returns>
        public static bool UpdateListWithAnother<T>(this List<T> list, List<T> otherList)
        {
            bool hasChanged = false;
            foreach (T instFromList in otherList.Where(r => !list.Contains(r)))
            {
                list.Add(instFromList);
                hasChanged = true;
            }

            foreach (T instFromCurr in list.ToList().Where(r => !otherList.Contains(r)))
            {
                list.Remove(instFromCurr);
                hasChanged = true;
            }

            return hasChanged;
        }

    }
}
