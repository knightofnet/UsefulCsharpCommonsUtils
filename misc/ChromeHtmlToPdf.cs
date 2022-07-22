using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.misc
{
    public class ChromeHtmlToPdf
    {

        public string ChromeExePath { get; set; }

        public bool NoHeaders { get; set; }
        public bool DisableGpu { get; set; }

        public Uri InputUri { get; set; }

        public Uri OutputPdfUri { get; set; }

        public ChromeHtmlToPdf(string chromeExePath)
        {
            NoHeaders = true;
            DisableGpu = true;

            ChromeExePath = chromeExePath;
        }

        public bool HtmlToPdf()
        {
            if (InputUri == null)
            {
                throw new Exception("InputUri n'est  pas définit");
            }
            if (OutputPdfUri == null)
            {
                throw new Exception("OutputPdfUri n'est  pas définit");
            }
            if (string.IsNullOrWhiteSpace(ChromeExePath) || !File.Exists(ChromeExePath))
            {
                throw new Exception("OutputPdfUri n'est  pas définit");
            }

            StringBuilder s = new StringBuilder($"--headless --print-to-pdf=\"{OutputPdfUri.AbsolutePath}\" ");
            if (NoHeaders)
            {
                s.Append("--print-to-pdf-no-header ");
            }

            if (DisableGpu)
            {
                s.Append("--disable-gpu ");
            }

            s.Append(InputUri.AbsoluteUri);

            return CommonsProcessUtils.DoCmd(ChromeExePath, s.ToString().Trim());

        }
    }
}
