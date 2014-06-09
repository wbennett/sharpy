using System;
using System.Runtime.Serialization;

namespace SharpNet.Service.Response
{
    [DataContract]
    public class ActivateResponse
    {
        [DataMember]
        public string SessionId { get; set; }
    }
}