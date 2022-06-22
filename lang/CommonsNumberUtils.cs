using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang
{
    public static class CommonsNumberUtils
    {

        public static int ParseOrDefault(string strToParse, int defaultValue = 0)
        {
            return int.TryParse(strToParse, out int retValue) ? retValue : defaultValue;
        }

        public static long ParseOrDefault(string strToParse, long defaultValue = 0)
        {
            return long.TryParse(strToParse, out long retValue) ? retValue : defaultValue;
        }

        public static short ParseOrDefault(string strToParse, short defaultValue = 0)
        {
            return short.TryParse(strToParse, out short retValue) ? retValue : defaultValue;
        }


    }
}
