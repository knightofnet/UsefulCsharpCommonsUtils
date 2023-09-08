using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly string _urlYt;
        private Dictionary<string, YoutrackObject> cacheModifYtO = new Dictionary<string, YoutrackObject>();

        private readonly CookieContainer cookieContainer;

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
                XElement xElement = XElement.Parse(raw);
                string type = xElement.DescendantsAndSelf("projectCustomField").FirstOrDefault()?.Attribute("type")?.Value;
                if (type != null)
                {
                    if (type.StartsWith("ownedField"))
                    {
                        string fieldNameBack =
                            xElement.Descendants("param").FirstOrDefault()?.Attribute("value")?.Value;
                        if (fieldNameBack != null)
                        {
                            fieldNameBack = HttpUtility.UrlEncode(fieldNameBack).Replace("+", "%20");
                            url = $"{_urlYt}/rest/admin/customfield/ownedFieldBundle/{fieldNameBack}";

                            raw = HttpGetCommand(new Uri(url), cookieContainer: cookieContainer);
                            xElement = XElement.Parse(raw);

                            if (xElement.DescendantsAndSelf("ownedFieldBundle").Any() && xElement.Descendants("ownedField").Any())
                            {
                                retList.AddRange(xElement.Descendants("ownedField").Select(field => field.Value));
                            }
                        }


                    } else if (type.StartsWith("enum"))
                    {
                        string fieldNameBack =
                            xElement.Descendants("param").FirstOrDefault()?.Attribute("value")?.Value;
                        if (fieldNameBack != null)
                        {
                            fieldNameBack = HttpUtility.UrlEncode(fieldNameBack).Replace("+", "%20");
                            url = $"{_urlYt}/rest/admin/customfield/bundle/{fieldNameBack}";

                            raw = HttpGetCommand(new Uri(url), cookieContainer: cookieContainer);
                            xElement = XElement.Parse(raw);

                            if (xElement.DescendantsAndSelf("enumeration").Any() && xElement.Descendants("value").Any())
                            {
                                retList.AddRange(xElement.Descendants("value").Select(field => field.Value));
                            }
                        }
                    }
                    else if (type.StartsWith("version"))
                    {

                    }
                    else if (type.StartsWith("user"))
                    {

                    }
                }


                //XmlFile xmlFile = XmlFile.InitXmlFileByString(raw);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return retList.ToArray();
        }

        public List<YoutrackObject> GetYoutracks(string filter)
        {
            string url = $"{_urlYt}/rest/issue?max=50&filter={HttpUtility.UrlEncode(filter)}";
            string data = string.Empty;

            List<YoutrackObject> list = new List<YoutrackObject>();

            try
            {
                string raw = HttpGetCommand(new Uri(url), data, cookieContainer: cookieContainer);
                XElement xElement = XElement.Parse(raw);
                //XmlFile xmlFile = XmlFile.InitXmlFileByString(raw);

                foreach (XElement xmlYtIssue in xElement.Descendants("issue"))
                {
                   
                    YoutrackObject ytORes = new YoutrackObject();
                   
                  
                    XmlToYoutrack(xmlYtIssue, ytORes);
                    ytORes.PropertyUpdated.Clear();


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
          
           

            List<string> propsToUpdate = youtrackObject.PropertyUpdated;
            string propYt = null;
            try
            {
                
                foreach (string propToUpdate in propsToUpdate)
                {
                    var ytAttr = GetYoutrackFieldAttribute(propToUpdate);
                    propYt = ytAttr.ElementRef;
                    UpdateField(ytAttr.ElementRef, youtrackObject.Id, (string)youtrackObject.GetType().GetProperty(propToUpdate).GetValue(youtrackObject));
                }

                youtrackObject.PropertyUpdated.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }


            return true;
        }

        public bool UpdateField(string fieldName, string issueId, string value)
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
                throw new Exception($"Exception when updating {fieldName} field.",ex);
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

                return xElt.Descendants("projectCustomField")
                    .Count(r => r.Attribute("name").Value.Equals(fieldName)) != 0;

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

        public YoutrackObject CreateIssue(string project, string summary, string description)
        {

            string url = $"{_urlYt}/rest/issue";

            var dict = new Dictionary<string, string>
            {
                { "project", project },
                { "summary", summary }
            };

            if (description != null)
            {
                dict["description"] = description;
            }

            try
            {
                string data = new FormUrlEncodedContent(dict).ReadAsStringAsync().Result;
                string raw = HttpPostCommand(new Uri(url), data , cookieContainer: cookieContainer);
                XElement xElt = XElement.Parse(raw);

                YoutrackObject yt = new YoutrackObject()
                {
                    Id = xElt.Attributes("id").FirstOrDefault()?.Value,
                    Summary = summary,
                    Project = project
                };
                yt.PropertyUpdated.Clear();
                


                return yt;


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



            ytO.Id = GetXmlYtValue(xmlNode,  nameof(YoutrackObject.Id));
            ytO.Project = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Project));
            ytO.Subsystem = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Subsystem));//  xmlNode.SelectSingleNode("./field[@name='Subsystem']/value[1]/text()")?.Value;
            ytO.FixVersion = GetXmlYtValue(xmlNode, nameof(YoutrackObject.FixVersion)); // xmlNode.SelectSingleNode("./field[@name='Fix versions']/value[1]/text()")?.Value;
            ytO.Summary = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Summary)); // xmlNode.SelectSingleNode("./field[@name='summary']/value[1]/text()")?.Value;
            //ytO.TypeYt = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Id)); // xmlNode.SelectSingleNode("./field[@name='Type']/value[1]/text()")?.Value;
            ytO.Sheet = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Sheet)); // xmlNode.SelectSingleNode("./field[@name='Sheet']/value[1]/text()")?.Value;
            ytO.Affectation = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Affectation)); // xmlNode.SelectSingleNode("./field[@name='Affectation']/value[1]/text()")?.Value;
            ytO.Demandeur = GetXmlYtValue(xmlNode, nameof(YoutrackObject.Demandeur)); // xmlNode.SelectSingleNode("./field[@name='Demandeur']/value[1]/text()")?.Value;

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
