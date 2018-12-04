using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using Ascentn.Workflow.Base;

namespace APAuthentication
{
    public class Program
    {
        public static void Main()
        {
            string prgtype = "REST";
            int i;
            GetUUID getidresult;
            
            string[] arruid = new string[4]; 
            string pdomain = ConfigurationManager.AppSettings["AP_Domain"];
            string puserName = ConfigurationManager.AppSettings["AP_UserName"];
            string dummyusername;
            string ppassword = ConfigurationManager.AppSettings["AP_Password"];
            string pappid = ConfigurationManager.AppSettings["AP_App_ID"];
            string plocale = ConfigurationManager.AppSettings["AP_Locale"];
            string getuiduri = ConfigurationManager.AppSettings["GetGUUID"];
            string getuuid = arruid[0];
            string ProcID = string.Empty;
            string RequesterName = "vaibhav";
            string RequestingAccessfor = "serita";
            string ProcInstID = string.Empty;
            string pcreateinst = ConfigurationManager.AppSettings["ProcessIns"];
            string GetReleasePID = ConfigurationManager.AppSettings["GetReleasePID"];
            string storeReleaseID = String.Empty;
            GetRPID JReleasePID;
           
            //  create GUUID

            if(prgtype== "WCF")
            {
                AgilePointHelper.InitiateReqeustSiteAccessWorkflow("vaibhav","vaibhav@agile.com","agilepoint","133124");  

            }
            else
            {

            
            #region REST API Call Create Proc Instance
            try
            {
                    HTTPOperations strName = new HTTPOperations(pdomain, puserName, ppassword, pappid, plocale);
                    // using REST API
                    for (i = 0; i <= 3; i++)
                    {
                    ProcID = strName.GetData(getuiduri);
                    getidresult = JsonConvert.DeserializeObject<GetUUID>(ProcID);
                    arruid[i] = getidresult.GetUUIDResult;
                    Console.WriteLine(arruid[i]);
                    }

                    storeReleaseID = strName.GetData(GetReleasePID);
                    JReleasePID = JsonConvert.DeserializeObject<GetRPID>(storeReleaseID);
                    Console.WriteLine(JReleasePID.GetReleasedPIDResult);

                Jsonde proc = new Jsonde();
                proc.ProcessID = JReleasePID.GetReleasedPIDResult;
                proc.ProcessInstID = arruid[1];
                proc.ProcInstName = "BarCodeProcess" + proc.ProcessInstID;
                proc.WorkObjID = arruid[2];
                proc.WorkObjInfo = null;
                proc.SuperProcInstID = null;
                proc.Initiator = pdomain + "//" + puserName;
                proc.CustomID = arruid[3];
                List<Attributes> nvp = new List<Attributes>();
                nvp.Add(new Attributes("/pd:AP/pd:formFields/pd:TextBox1", RequesterName));
               // nvp.Add(new Attributes("/pd:AP/pd:formFields/pd:frmAccessForName", RequestingAccessfor));
                proc.Attributes = nvp.ToArray();
                proc.blnStartImmediately = true;
                string fjson = JsonConvert.SerializeObject(proc);
                    Console.WriteLine(fjson);
                //Json String
                string scJson = "{\"ProcessID\":\""+JReleasePID.GetReleasedPIDResult+"\"," +
                    "\"ProcessInstID\":\""+arruid[1]+"\"," +
                    "\"ProcInstName\":\"BarCodeProcess-" + arruid[1] + "\"," +
                    "\"WorkObjID\":\"" + arruid[2] + "\"," +
                    "\"WorkObjInfo\":\"" + null + "\"," +
                    "\"SuperProcInstID\":\"" + null + "\"," +
                    "\"Initiator\":\""+pdomain+ "" +puserName+"\"," +
                    "\"CustomID\":\"" + arruid[3] + "\"," +
                    "\"Attributes\":[{\"Name\":\"/pd:AP/pd:formFields/pd:TextBox1\"," +
                    "\"Value\":\"Tom\"}]," +
                    "\"blnStartImmediately\":true}";
                Console.WriteLine(scJson);
                strName.POSTMethod(pcreateinst, scJson);
            }
            catch (Exception ex)
            {

                throw ex;
            }
                #endregion
                //Console.WriteLine(getidresult.GetUUIDResult);
                //getuuid = getidresult.GetUUIDResult;
                #region InitiateReqeustSiteAccessWorkflow
                //try
                //{
                //    AgilePointHelper.InitiateReqeustSiteAccessWorkflow(RequesterName, RequestingAccessfor);
                //    Console.WriteLine("Process working");
                //}
                //catch (Exception ex)
                //{

                //    throw ex;
                //}
                #endregion
                //GetProcess Instance Attributes
                #region Deserializing
                /*ProcID = strName.GetData(getuiduri);
                getidresult = JsonConvert.DeserializeObject<GetUUID>(ProcID);
                string guid = getidresult.GetUUIDResult;

                Console.WriteLine(guid);*/
                #endregion
                #region GetProcessInstAttribute
                //string URL = "https://trialas2.nxone.com:443/AgilePointServer/Workflow/GetProcInstAttrs/81310EDCEE472F8411886B7D15D741CA";
                //string store = strName.GetData(URL);
                //Console.WriteLine(store);

                ////Dataset deserializing
                //DataSet dataset = JsonConvert.DeserializeObject<DataSet>(store);
                //DataTable dataTable = dataset.Tables["GetProcInstAttrsResult"];
                //Console.WriteLine(dataTable.Rows.Count);
                //foreach (DataRow rows in dataTable.Rows)
                //{
                //    Console.WriteLine(rows["name"] + "-" + rows["value"]);
                //}
                #endregion

                #region GetCustomAttributebyID
                //string getcustattrs = "https://trialas2.nxone.com:443/AgilePointServer/Workflow/GetCustomAttrsByID/81310EDCEE472F8411886B7D15D741CA";
                //string storeresp = strName.GetData(getcustattrs);
                //Console.WriteLine(storeresp);
                //Dataset deserializing

                #endregion

                #region GetProcessInstAttributebyName



                #endregion
            }
        }


    }
}
