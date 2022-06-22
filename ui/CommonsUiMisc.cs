using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UsefulCsharpCommonsUtils.ui
{
    public static class CommonsUiMisc
    {

        public static string GetContent(this System.Windows.Documents.Hyperlink h)
        {
            return h.Inlines.ToString();
        }

        public static void SetContent(this System.Windows.Documents.Hyperlink h, string s) {
            h.Inlines.Clear();
            h.Inlines.Add(s);
        }

    }
}
