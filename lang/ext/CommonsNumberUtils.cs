using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang.ext
{
    public static class CommonsNumberUtils
    {
        public static int ParseOrDefault(this string strToParse, int defaultValue = 0)
        {
            return lang.CommonsNumberUtils.ParseOrDefault(strToParse, defaultValue);
        }

        public static long ParseOrDefault(this string strToParse, long defaultValue = 0)
        {
            return lang.CommonsNumberUtils.ParseOrDefault(strToParse, defaultValue);
        }

        public static short ParseOrDefault(this string strToParse, short defaultValue = 0)
        {
            return lang.CommonsNumberUtils.ParseOrDefault(strToParse, defaultValue);
        }
    }
}
