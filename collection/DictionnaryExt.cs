using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.collection
{
    public static class DictionnaryExt
    {

        public static void AddAndReplace<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue value)
        {
            if (d.ContainsKey(key))
            {
                d.Remove(key);
            }
            d.Add(key, value);
        }

        public static void AddNew<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue value)
        {
            if (!d.ContainsKey(key))
            {
                d.Add(key, value);
            }
           
        }

        
    }


}
