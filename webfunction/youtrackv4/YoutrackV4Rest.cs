using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using UsefulCsharpCommonsUtils.lang;
using UsefulCsharpCommonsUtils.webfunction;
using static UsefulCsharpCommonsUtils.webfunction.WebFunctionsUtils;


namespace UsefulCsharpCommonsUtils.webfunction.youtrackv4
{

    public class YoutrackV4Rest
    {
        private string _urlYt;
        private Dictionary<string, YoutrackObject> cacheModifYtO = new Dictionary<string, YoutrackObject>();

        private CookieContainer cookieContainer;

        public YoutrackV4Rest(string urlYt)
        {
            _urlYt = urlYt;
            cookieContainer = new CookieContainer();
        }

        public void ClearCache()
        {
            cacheModifYtO.Clear();
        }

        public bool Login(string user, string password)
        {
            string url = $"{_urlYt}/rest/user/login";
            string data = $"login={user}&password={password}";

            try
            {
                HttpPostCommand(new Uri(url), data, cookieContainer: cookieContainer);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }


        }

        public string[] GetEnumFieldValues(string projetId, string fieldName)
        {
            if (!IsFieldExists(projetId, fieldName)) return null;

            List<string> retList = new List<string>();

            string url = $"{_urlYt}/rest/admin/project/{projetId}/customfield/{HttpUtility.UrlEncode(fieldName)}";

            try
            {
                string raw = HttpGetCommand(new Uri(url), cookieContainer: cookieContainer);
                //XmlFile xmlFile = XmlFile.InitXmlFileByString(raw);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return null;
        }

        public List<YoutrackObject> GetYoutracks(string filter)
        {
            string url = $"{_urlYt}/rest/issue?filter={HttpUtility.UrlEncode(filter)}";
            string data = string.Empty;

            List<YoutrackObject> list = new List<YoutrackObject>();

            try
            {
                string raw = HttpGetCommand(new Uri(url), data, cookieContainer: cookieContainer);
                XElement xElement = XElement.Parse(raw);
                //XmlFile xmlFile = XmlFile.InitXmlFileByString(raw);

                foreach (XElement xmlYtIssue in xElement.Descendants("issue"))
                {
                    YoutrackObject ytOriginal = new YoutrackObject();
                    YoutrackObject ytORes = new YoutrackObject();
                   
                    XmlToYoutrack(xmlYtIssue, ytOriginal);
                    XmlToYoutrack(xmlYtIssue, ytORes);

                   string specId = CommonsStringUtils.RandomString(16, ensureUnique: true);
                    ytOriginal.SpecialId = specId;
                    ytORes.SpecialId = specId;

                    cacheModifYtO.Add(specId, ytOriginal);
                   
                    list.Add(ytORes);
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }

            return list;
        }



        public bool UpdateYoutrack(YoutrackObject youtrackObject)
        {
            if (youtrackObject == null) return false;
            if (!cacheModifYtO.ContainsKey(youtrackObject.SpecialId))
            {
                throw new Exception("Yt incorrect");
            }

            YoutrackObject original = cacheModifYtO[youtrackObject.SpecialId];

            List<string> propsToUpdate = new List<string>();

            try
            {
                if (!LangUtils.IsEqWithNull(original.Subsystem, youtrackObject.Subsystem)) propsToUpdate.Add("Subsystem");
                if (!LangUtils.IsEqWithNull(original.FixVersion, youtrackObject.FixVersion)) propsToUpdate.Add("FixVersion");
                if (!LangUtils.IsEqWithNull(original.Type, youtrackObject.Type)) propsToUpdate.Add("Type");
                if (!LangUtils.IsEqWithNull(original.Sheet, youtrackObject.Sheet)) propsToUpdate.Add("Sheet");
                if (!LangUtils.IsEqWithNull(original.Affectation, youtrackObject.Affectation)) propsToUpdate.Add("Affectation");
                if (!LangUtils.IsEqWithNull(original.Demandeur, youtrackObject.Demandeur)) propsToUpdate.Add("Demandeur");

                foreach (string propToUpdate in propsToUpdate)
                {
                    var ytAttr = GetYoutrackFieldAttribute(propToUpdate);
                    UpdateField(ytAttr.ElementRef, youtrackObject.Id, (string)youtrackObject.GetType().GetProperty(propToUpdate).GetValue(youtrackObject));
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }


            return true;
        }

        private bool UpdateField(string fieldName, string issueId, string value)
        {
            string url = $"{_urlYt}/rest/issue/{issueId}/execute";
            string data = $"command={fieldName}%20{value}";

            try
            {
                HttpPostCommand(new Uri(url), data, cookieContainer: cookieContainer);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                // return false;
            }

        }

        public bool IsFieldExists(string project, string fieldName)
        {
            string url = $"{_urlYt}/rest/admin/project/{project}/customfield";


            try
            {
                string raw = HttpGetCommand(new Uri(url), cookieContainer: cookieContainer);
                XElement xElt = XElement.Parse(raw);

                return xElt.Descendants($"/projectCustomFieldRefs/projectCustomField[@name='{fieldName}']") != null;

                /*
                XmlFile xmlFile = XmlFile.InitXmlFileByString(raw);

                return xmlFile.Root.SelectSingleNode($"/projectCustomFieldRefs/projectCustomField[@name='{fieldName}']") != null;
                */
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }

        }



        private static void XmlToYoutrack(XElement xmlNode, YoutrackObject ytO)
        {
            //YoutrackFieldAttribute youtrackFieldAttribute_Id = GetYoutrackFieldAttribute("Id");



            ytO.Id = GetXmlYtValue(xmlNode, "Id");
            ytO.Project = GetXmlYtValue(xmlNode, "Project");
            ytO.Subsystem = GetXmlYtValue(xmlNode, "Subsystem");//  xmlNode.SelectSingleNode("./field[@name='Subsystem']/value[1]/text()")?.Value;
            ytO.FixVersion = GetXmlYtValue(xmlNode, "FixVersion"); // xmlNode.SelectSingleNode("./field[@name='Fix versions']/value[1]/text()")?.Value;
            ytO.Summary = GetXmlYtValue(xmlNode, "Summary"); // xmlNode.SelectSingleNode("./field[@name='summary']/value[1]/text()")?.Value;
            ytO.Type = GetXmlYtValue(xmlNode, "Type"); // xmlNode.SelectSingleNode("./field[@name='Type']/value[1]/text()")?.Value;
            ytO.Sheet = GetXmlYtValue(xmlNode, "Sheet"); // xmlNode.SelectSingleNode("./field[@name='Sheet']/value[1]/text()")?.Value;
            ytO.Affectation = GetXmlYtValue(xmlNode, "Affectation"); // xmlNode.SelectSingleNode("./field[@name='Affectation']/value[1]/text()")?.Value;
            ytO.Demandeur = GetXmlYtValue(xmlNode, "Demandeur"); // xmlNode.SelectSingleNode("./field[@name='Demandeur']/value[1]/text()")?.Value;

        }

        private static string GetXmlYtValue(XElement xmlNode, string v)
        {
            YoutrackFieldAttribute ytf = GetYoutrackFieldAttribute(v);
            if (ytf == null) return null; //typeof(YoutrackObject).GetProperty(v).CustomAttributes.FirstOrDefault(r=>typeof(r) == typeof(YoutrackFieldAttribute))

            if (ytf.IssueElementType == YoutrackFieldAttribute.YoutrackIssueElt.ATTR)
            {
                return xmlNode.Attribute(ytf.ElementRef)?.Value;
            }
            else if (ytf.IssueElementType == YoutrackFieldAttribute.YoutrackIssueElt.FIELD)
            {
                return xmlNode.Elements("field").FirstOrDefault(r => (string)r.Attribute("name") == ytf.ElementRef)?.Value;
            }
            else if (ytf.IssueElementType == YoutrackFieldAttribute.YoutrackIssueElt.CFIELD)
            {
                return xmlNode.Elements("field").FirstOrDefault(r => (string)r.Attribute("name") == ytf.ElementRef)?.Value;
            }

            return null;

        }

        private static YoutrackFieldAttribute GetYoutrackFieldAttribute(string v)
        {
            return typeof(YoutrackObject).GetProperty(v).GetCustomAttributes(false).Cast<YoutrackFieldAttribute>().FirstOrDefault();
            //return typeof(YoutrackObject).GetProperty(v).CustomAttributes.FirstOrDefault(r => r.AttributeType.Name.Equals("YoutrackFieldAttribute"));
        }
    }
}
