using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascentn.AgilePoint.WCFClient;
using Ascentn.Workflow.Base;

namespace APAuthentication
{
   public class AgilepointWCFCaller
    {
       public AgilepointWCFCaller()
        {
            IWFWorkflowService api = AgilePointHelper.GetAPI();
            string procID = api.GetReleasedPID(ConfigurationManager.AppSettings["AP_ApplicationName"]);
        }
    }
}
