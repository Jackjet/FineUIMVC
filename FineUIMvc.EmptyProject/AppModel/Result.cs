using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FineUIMvc.PumpMVC.AppModel
{

    [DataContract]
    public class Result
    {

        [DataMember]
        public string ErrorCode
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }

        [DataMember]
        public int Status
        {
            get;
            set;
        }
    }

}