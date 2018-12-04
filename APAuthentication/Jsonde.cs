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
using System.Runtime.Serialization;

namespace APAuthentication
{
    [DataContract]
    public class Jsonde
    {
        [DataMember]
        public string ProcessID { get; set; }
        [DataMember]
        public string ProcessInstID { get; set; }
        [DataMember]
        public string ProcInstName { get; set; }
        [DataMember]
        public string WorkObjID { get; set; }
        [DataMember]
        public string WorkObjInfo { get; set; }
        [DataMember]
        public string SuperProcInstID { get; set; }
        [DataMember]
        public string Initiator { get; set; }
        [DataMember]
        public string CustomID { get; set; }
        [DataMember]
        public Attributes[] Attributes {get; set;} 
        [DataMember]
        public bool blnStartImmediately { get; set; }
    }

    [DataContract]
    public class Attributes
    {
        public Attributes (string name, object val)
        {
            Name = name;
            Value = val;
        }
        
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public object Value { get; set; }

       
    }

    [DataContract]
    public class GetUUID
    {
        [DataMember]
        public string GetUUIDResult { get; set; }
    }

    [DataContract]
    public class GetRPID
    {
        [DataMember]
        public string GetReleasedPIDResult { get; set; }
    }
}