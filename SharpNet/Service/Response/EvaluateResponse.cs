using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SharpNet.Service.Response
{
    [DataContract]
    public class EvaluateResponse
    {
        [DataMember]
        public string StandardOut { get; set; }
        [DataMember]
        public string StandardError { get; set; }
        [DataMember]
        public string ReturnValue { get; set; }
        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember]
        public string SessionId { get; set; }
    }
}