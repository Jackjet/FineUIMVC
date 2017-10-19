using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FineUIMvc.PumpMVC.AppModel
{
    [DataContract]
    public class LoginInfo : Result
    {

        [DataMember]
        public string TokenID
        {
            get;
            set;

        }
        [DataMember]
        public string UserID
        {
            get;
            set;

        }
    }
}