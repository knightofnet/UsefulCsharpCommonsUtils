using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UsefulCsharpCommonsUtils.webfunction.jenkins.dto;


namespace UsefulCsharpCommonsUtils.webfunction.jenkins
{
    public class JenkinsClient
    {
        public string User { get; }
        public string PwdToken { get; }
        public Uri JenkinsUri { get; }

        public string Crumb { get; }
        public string CrumbRequestField { get; }

        public bool IsConnected { get; }

        public JenkinsClient(string user, string pwdToken, Uri jenkinsUri)
        {
            User = user;
            PwdToken = pwdToken;
            JenkinsUri = jenkinsUri;

            string[] rawCrumb = GetCrumb();
            CrumbRequestField = rawCrumb[0];
            Crumb = rawCrumb[1];

            IsConnected = true;
        }

        public WorkflowRun GetLastBuild(string jobRelUrl)
        {
            Uri uri = new Uri((new Uri(JenkinsUri, jobRelUrl)).AbsoluteUri + "/lastBuild/api/json");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);

            wr.Proxy = WebRequest.GetSystemWebProxy();
            wr.Method = "GET";
            wr.Credentials = new NetworkCredential(User, PwdToken);

            WebFunctionsUtils.ExecRequest(wr, "");

            JObject jsonJObject = WebFunctionsUtils.GetWebResponseAsJson(wr);

            WorkflowRun run = new WorkflowRun()
            {
                BuildId = jsonJObject["id"].Value<string>(),
                BuildNumber = jsonJObject["number"].Value<int>(),
                Result = jsonJObject["result"].Value<string>(),
                Url = new Uri(jsonJObject["url"].Value<string>()),
                Building = jsonJObject["building"].Value<bool>(),
            };

            /*
             // TODO
            foreach (JToken jToken in jsonJObject["actions"][0]["parameters"])
            {
                KeyValuePair<string, string> kv = new KeyValuePair<string, string>()

            }
            */

            return run;

        }

        public bool SubmitJobNoParam(string jobRelUrl)
        {
            return SubmitJobWithParams(jobRelUrl, null);
        }

        public bool SubmitJobWithParams(string jobRelUrl, Dictionary<string, string> jobParams)
        {

            Uri uri = new Uri((new Uri(JenkinsUri, jobRelUrl)).AbsoluteUri + "/buildWithParameters");

            string paramUrl = WebFunctionsUtils.GetParamsUrlFromDictionnary(jobParams);

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri + paramUrl);
            wr.Proxy = WebRequest.GetSystemWebProxy();
            wr.Method = "POST";
            wr.Credentials = new NetworkCredential(User, PwdToken);


            wr.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{User}:{PwdToken}")));
            wr.Headers.Add(CrumbRequestField, Crumb);

            WebFunctionsUtils.ExecRequest(wr, string.Empty);


            string rep = WebFunctionsUtils.GetWebResponse(wr, out WebResponse webResponse);

            return ((HttpWebResponse)webResponse).StatusDescription.Equals("Created");
        }

        private string[] GetCrumb()
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(new Uri(JenkinsUri, "crumbIssuer/api/json"));
            wr.Proxy = WebRequest.GetSystemWebProxy();
            wr.Method = "GET";
            wr.Credentials = new NetworkCredential(User, PwdToken);

            WebFunctionsUtils.ExecRequest(wr, "");

            JObject jsonJObject = WebFunctionsUtils.GetWebResponseAsJson(wr);
            //JObject jsonJObject = JObject.Parse(rep);
            if (jsonJObject == null || !jsonJObject.ContainsKey("crumb") || string.IsNullOrWhiteSpace(jsonJObject["crumb"].Value<string>()))
            {
                // TODO log
                throw new Exception("Erreur : pas de crumb récupéré sur Jenkins");
            }
            return new string[] { jsonJObject["crumbRequestField"].Value<string>(), jsonJObject["crumb"].Value<string>() };

        }
    }
}
