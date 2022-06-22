using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.webfunction.jenkins.dto
{
    public class WorkflowRun
    {
        public int BuildNumber { get; set; }
        public string BuildId { get; set; }

        public Uri Url { get; set; }

        public string Result { get; set; }

        public bool Building { get; set; }

        public KeyValuePair<string, string>[] Parameters { get; set; }

    }
}
