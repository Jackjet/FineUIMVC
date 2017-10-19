using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace FineUIMvc.PumpMVC.AppModel
{
    [DataContract]
    public class AcceptLoginInfo
    {

        [DataMember]
        public string UserName
        {
            get;
            set;

        }
        [DataMember]
        public string Md5
        {
            get;
            set;
        }
    }
}