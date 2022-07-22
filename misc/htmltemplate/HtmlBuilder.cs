using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UsefulCsharpCommonsUtils.misc.htmltemplate
{
    public class HtmlBuilder
    {
        private readonly string _tagTpl = "{{{{{0}}}}}";

        public string OriginalHtml { get; private set; }
        public string NewHtml { get; private set; }

        public HtmlBuilder(string htmlContent)
        {
            OriginalHtml = htmlContent;
            NewHtml = OriginalHtml;
        }

        public string FillHtmlWithObject(object obj, Type typeObj, Dictionary<string, string> fieldsToUseByTargetTags)
        {

            foreach (KeyValuePair<string, string> kv in fieldsToUseByTargetTags)
            {
                string tag = kv.Key;
                string field = kv.Value;

                object value = typeObj.GetProperty(field).GetValue(obj, null);

                if (value == null) continue;

                string tagReal = string.Format(_tagTpl, tag);
                string valueStr = GetValueStr(value);



            }

            return NewHtml;
        }




        public string FillHtmlWith(params object[] kv)
        {
            if (kv.Length % 2 != 0)
            {
                throw new Exception("It must be tag:obj pair");
            }

            Dictionary<string, string> dicoFill = new Dictionary<string, string>();

            string currentKey = null;
            for (int i = 0; i < kv.Length; i++)
            {
                object elt = kv[i];
                if (i % 2 == 0)
                {
                    if (elt is string s)
                    {
                        currentKey = s;
                    }
                    else
                    {
                        throw new Exception("Pair element must be a string");
                    }
                }
                else
                {
                    dicoFill.Add(currentKey, GetValueStr(elt));
                    currentKey = null;
                }
            }

            return FillHtmlWith(dicoFill);
        }

        public string FillHtmlWith(Dictionary<string, string> fieldValueByTags)
        {

            foreach (KeyValuePair<string, string> kv in fieldValueByTags)
            {
                string tag = kv.Key;
                string value = kv.Value;

                string tagReal = string.Format(_tagTpl, tag);
                NewHtml = NewHtml.Replace(tagReal, (string)value);

            }

            return NewHtml;
        }

        private static string GetValueStr(object value)
        {
            switch (value)
            {
                case string s:
                    return s;
                case int _:
                case float _:
                case double _:
                case decimal _:
                    return value.ToString();
                case DateTime time:
                    return time.ToString("g");
                case HtmlBuilder h:
                    return h.NewHtml;
                    break;
                default:
                    return value.ToString();
            }
        }



        public void SaveNew(string outFilename)
        {
            File.WriteAllText(outFilename, NewHtml, Encoding.UTF8);
        }





        public static HtmlBuilder CreateNewByFilepath(string filePath)
        {
            return CreateNewByFilepath(filePath, Encoding.UTF8);
        }

        public static HtmlBuilder CreateNewByFilepath(string filePath, Encoding encoding)
        {
            string htmlContent = File.ReadAllText(filePath, encoding);
            return new HtmlBuilder(htmlContent);
        }


        public static string ForListTag<T>(IEnumerable<T> toList, string beforeElt = "", string afterElt = "",
            string before = "", string after = "", string contentIfEmpty = "")

        {
            StringBuilder sb = new StringBuilder();
            var enumerable = toList as T[] ?? toList.ToArray();

            if (!enumerable.Any())
            {
                sb.AppendFormat("{0}{1}{2}", beforeElt, contentIfEmpty, afterElt);
            }
            foreach (T elt in enumerable)
            {
                sb.AppendFormat("{0}{1}{2}", beforeElt, GetValueStr(elt), afterElt);
            }

            sb.AppendLine(after);

            return sb.ToString();
        }
    }
}


