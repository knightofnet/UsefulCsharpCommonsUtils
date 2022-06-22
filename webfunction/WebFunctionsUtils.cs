using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace UsefulCsharpCommonsUtils.webfunction
{
    public static class WebFunctionsUtils
    {
        public static CookieContainer CurrentStaticCookieContainer { get; private set; }

        public enum HttpRequestMethods
        {
            POST,
            GET,
            PUT
        }

        public static HttpWebRequest GetWebRequest(Uri url, HttpRequestMethods method, string contentType)
        {
            IWebProxy proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = method.ToString();

            if (!string.IsNullOrEmpty(contentType))
            {
                wr.ContentType = contentType;
            }

            wr.Proxy = proxy;

            return wr;

        }

        internal static void ExecRequest(HttpWebRequest wr, string data)
        {

            byte[] bodyBytes = Encoding.UTF8.GetBytes(data);
            if (wr.Method == HttpRequestMethods.POST.ToString())
            {
                using (var request = wr.GetRequestStream())
                {
                    request.Write(bodyBytes, 0, bodyBytes.Length);
                    //request.Flush();
                }
            }

        }

        internal static string GetWebResponse(HttpWebRequest wr)
        {
            return GetWebResponse(wr, out WebResponse rep);
        }

        internal static string GetWebResponse(HttpWebRequest wr, out WebResponse webResponse)
        {
            try
            {
                webResponse = wr.GetResponse();
                if (webResponse == null) return null;

                string result = null;
                using (var request = webResponse.GetResponseStream())
                    if (request != null)
                    {
                        using (var sr = new StreamReader(request))
                        {
                            result = sr.ReadToEnd();
                        }
                    }

                return result;
            }
            catch (WebException e)
            {
                Console.WriteLine("This program is expected to throw WebException on successful run." +
                                  "\n\nException Message :" + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    using (Stream datas = e.Response.GetResponseStream())
                    using (var reader = new StreamReader(datas))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }

                throw e;
            }

        }

        public static string HttpPostCommand(Uri url, string data,
            string contentType = "application/x-www-form-urlencoded", CookieContainer cookieContainer = null)
        {
            HttpWebRequest wr = GetWebRequest(url, HttpRequestMethods.POST, contentType);

            if (cookieContainer != null)
            {
                wr.CookieContainer = cookieContainer;
            }
            else
            {
                if (CurrentStaticCookieContainer == null)
                {
                    CurrentStaticCookieContainer = new CookieContainer();
                }

                wr.CookieContainer = CurrentStaticCookieContainer;
            }

            ExecRequest(wr, data);
            return GetWebResponse(wr);
        }

        public static string HttpGetCommand(Uri url, string data = "", CookieContainer cookieContainer = null)
        {
            HttpWebRequest wr = GetWebRequest(url, HttpRequestMethods.GET, "");
            if (cookieContainer != null)
            {
                wr.CookieContainer = cookieContainer;
            }
            else
            {
                if (CurrentStaticCookieContainer == null)
                {
                    CurrentStaticCookieContainer = new CookieContainer();
                }

                wr.CookieContainer = CurrentStaticCookieContainer;
            }

            StringBuilder cookies = new StringBuilder();
            foreach (var cookie in wr.CookieContainer.GetCookies(url))
            {
                cookies.Append(cookie.ToString() + ";"); // test=testValue
            }

            wr.Headers.Add("Cookie", cookies.ToString().TrimEnd(';'));


            ExecRequest(wr, data);
            return GetWebResponse(wr);
        }

        public static bool IsValidWebResponse(string url, int expectedWr = 200, int timeout = 10000)
        {
            int statusNumber = -1;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowAutoRedirect = false;
            webRequest.UseDefaultCredentials = true;

            HttpWebResponse response = null;

            try
            {
                webRequest.Timeout = timeout;
                response = (HttpWebResponse)webRequest.GetResponse();
                // This will have statii from 200 to 30x
                statusNumber = (int)response.StatusCode;
            }
            catch (WebException we)
            {
                if (response == null)
                {
                    statusNumber = 0;
                }
                else
                {
                    // Statii 400 to 50x will be here
                    statusNumber = (int)((HttpWebResponse)we.Response).StatusCode;
                }
            }

            //_log_.Debug("IsValidWebResponse(url: {0}, expectedWr: {1}) => statusRep: {2}", url, expectedWr, statusNumber);

            return statusNumber == expectedWr;
        }

        public static JObject GetWebResponseAsJson(HttpWebRequest wr)
        {
            string rawResp = GetWebResponse(wr);
            return JObject.Parse(rawResp);
        }

        public static string GetParamsUrlFromDictionnary(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || !dictionary.Any()) return string.Empty;

            return "?" + string.Join("&",
                dictionary.Select(r => $"{HttpUtility.UrlEncode(r.Key)}={HttpUtility.UrlEncode(r.Value)}"));
        }

    }

}
