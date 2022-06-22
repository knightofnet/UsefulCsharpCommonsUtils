using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UsefulCsharpCommonsUtils.webfunction.youtrackv4.YoutrackFieldAttribute;

namespace UsefulCsharpCommonsUtils.webfunction.youtrackv4
{
    public class YoutrackObject
    {


        [YoutrackField("id", YoutrackIssueElt.ATTR)]
        public string Id { get; set; }

        [YoutrackField("projectShortName", YoutrackIssueElt.FIELD)]
        public string Project { get; set; }

        [YoutrackField("Subsystem", YoutrackIssueElt.CFIELD)]
        public string Subsystem { get; set; }

        [YoutrackField("Fix versions", YoutrackIssueElt.CFIELD)]
        public string FixVersion { get; set; }

        [YoutrackField("summary", YoutrackIssueElt.CFIELD)]
        public string Summary { get; set; }

        [YoutrackField("Type", YoutrackIssueElt.CFIELD)]
        public string Type { get; set; }

        [YoutrackField("Sheet", YoutrackIssueElt.CFIELD)]
        public string Sheet { get; set; }

        [YoutrackField("Affectation", YoutrackIssueElt.CFIELD)]
        public string Affectation { get; set; }

        [YoutrackField("Demandeur", YoutrackIssueElt.CFIELD)]
        public string Demandeur { get; set; }
        public string SpecialId { get; internal set; }
        //public string TypeSubsystem { get; set; }

    }
}
