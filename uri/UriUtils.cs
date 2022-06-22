using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.uri
{
    public static class UriUtils
    {

        public static string JoinUriSegments(string uri, params string[] segments)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return null;

            if (segments == null || segments.Length == 0)
                return uri;

            return segments.Aggregate(uri, (current, segment) => $"{current.TrimEnd('/')}/{segment.TrimStart('/')}");
        }

    }
}
