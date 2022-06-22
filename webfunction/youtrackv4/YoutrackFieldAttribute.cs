using System;

namespace UsefulCsharpCommonsUtils.webfunction.youtrackv4
{
    public class YoutrackFieldAttribute : Attribute
    {
        public YoutrackFieldAttribute(string elementRef, YoutrackIssueElt issueElementType)
        {
            ElementRef = elementRef;
            IssueElementType = issueElementType;
        }

        public string ElementRef { get; }
        public YoutrackIssueElt IssueElementType { get; }

        public enum YoutrackIssueElt
        {
            ATTR,
            CFIELD,
            FIELD
        }

    }
}