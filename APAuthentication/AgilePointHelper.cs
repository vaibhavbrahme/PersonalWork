using Ascentn.AgilePoint.WCFClient;
using Ascentn.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace APAuthentication
{
    public class AgilePointHelper
    {
        public static IWFWorkflowService GetAPI()
        {
           
            try
            {
                System.Configuration.AppSettingsReader appSettings = new System.Configuration.AppSettingsReader();
                string workFlowBinding = (string)appSettings.GetValue("WorkFlowBindingUsed", typeof(System.String));

                string _auth = string.Format("{0}:{1}", ConfigurationManager.AppSettings["AP_UserName"], ConfigurationManager.AppSettings["AP_Password"]);
                string _enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));
                string cred = string.Format("{0} {1}", "Basic", _enc);

                IWFWorkflowService api = null;
                string authenticatedUserName = "";

                string user = ConfigurationManager.AppSettings["AP_UserName"];
                api = new WCFWorkflowProxy(ConfigurationManager.AppSettings["AP_App_ID"], "", "en-US", user, cred, workFlowBinding);
                authenticatedUserName = api.CheckAuthenticated();
                return api;
            }
            catch (Exception)
            {

                throw;
            }
            

            
        }

        public static IWFAdminService GetADM()
        {
            System.Configuration.AppSettingsReader appSettings = new System.Configuration.AppSettingsReader();
            string adminBinding = (string)appSettings.GetValue("AdminBindingUsed", typeof(System.String));

            string _auth = string.Format("{0}:{1}", ConfigurationManager.AppSettings["AP_UserName"], ConfigurationManager.AppSettings["AP_Password"]);
            string _enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));
            string cred = string.Format("{0} {1}", "Basic", _enc);

            IWFAdminService adm = null;
            string authenticatedUserName = "";

            string user = ConfigurationManager.AppSettings["AP_UserName"];
            adm = new WCFAdminProxy(ConfigurationManager.AppSettings["AP_App_ID"], "", "en-US", user, cred, adminBinding);
            authenticatedUserName = adm.CheckAuthenticated();

            return adm;
        }

        public static WFEvent WaitUntilEventIsProcessed(WFEvent e, IWFWorkflowService wf, int sleepIntervalInMillis)
        {
            if (e == null)
            {
                throw new System.ArgumentNullException("e");
            }

            while (e.Status == WFEvent.SENT)
            {
                System.Threading.Thread.Sleep(sleepIntervalInMillis);
                e = wf.GetEvent(e.EventID);
            }

            if (e.Status == WFEvent.PROCESSED)
            {
                return e;
            }

            else
            {
                throw new System.Exception(e.Error);
            }
        }
        
        #region Training
        //public static void InitiateNotifyTrainingManagerWorkflow(string SharePointTopic, string BotUserID)
        //{
        //    // Create Workflow API Object
        //    IWFWorkflowService api = AgilePointHelper.GetAPI();

        //    // Get PID of the latest version of the process model
        //    string procDefID = api.GetReleasedPID(Settings.GetTrainingProcessModelName());

        //    // Create a unique Process instance name
        //    string piName = Settings.GetTrainingProcessModelName() + " - " + DateTime.Now;

        //    // Generate a GUID for work object ID
        //    string workObjId = UUID.GetID();

        //    //generate a GUID for process instance ID
        //    string procInstID = UUID.GetID();

        //    // Initialize some data
        //    List<NameValue> nvs = new List<NameValue>();
        //    nvs.Add(new NameValue("/pd:AP/pd:processFields/pd:SharePointTopic", SharePointTopic));
        //    nvs.Add(new NameValue("/pd:AP/pd:processFields/pd:RequestedUser", BotUserID));

        //    // Create process instance
        //    WFEvent evt = api.CreateProcInstEx(procDefID, procInstID, piName, workObjId, null, workObjId, nvs.ToArray(), true);

        //    // Wait for event to be processed 
        //    WFEvent evtResult = null;
        //    evtResult = WaitUntilEventIsProcessed(evt, api, 500);

        //}
        #endregion

        #region Request site access workflow
        public static void InitiateReqeustSiteAccessWorkflow(string RequesterName, string EmailID, string CompanyName, string ContactNo)
        {
            // Create Workflow API Object
            IWFWorkflowService api = AgilePointHelper.GetAPI();


            // Get PID of the latest version of the process model AP_Process_Name
            string procDefID = api.GetReleasedPID(ConfigurationManager.AppSettings["AP_Process_Name"]);

            // Create a unique Process instance name
            string piName = ConfigurationManager.AppSettings["AP_Process_Name"] + " - " + DateTime.Now;

            // Generate a GUID for work object ID
            string workObjId = UUID.GetID();

            //generate a GUID for process instance ID
            string procInstID = UUID.GetID();

            //${/pd:AP/pd:formFields/pd:frmRequesterName}
            // Initialize some data
            List<NameValue> nvs = new List<NameValue>();
            //${/pd:AP/pd:formFields/pd:Name}
            //${/pd:AP/pd:formFields/pd:EmailID}
            //${/pd:AP/pd:formFields/pd:CompanyName}
            //${/pd:AP/pd:formFields/pd:ContactNo}
            nvs.Add(new NameValue("/pd:AP/pd:formFields/pd:Name", RequesterName));
            nvs.Add(new NameValue("/pd:AP/pd:formFields/pd:EmailID", EmailID));
            nvs.Add(new NameValue("/pd:AP/pd:formFields/pd:CompanyName",CompanyName));
            nvs.Add(new NameValue("/pd:AP/pd:formFields/pd:ContactNo",ContactNo));
            
            
            // Create process instance
            WFEvent evt = api.CreateProcInstEx(procDefID, procInstID, piName, workObjId, null, workObjId, nvs.ToArray(), true);

            // Wait for event to be processed 
            WFEvent evtResult = null;
            evtResult = WaitUntilEventIsProcessed(evt, api, 500);

        }
        #endregion

        #region Search process instance info

        #endregion
    }

}