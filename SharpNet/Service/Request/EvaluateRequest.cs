using System;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using SharpNet.Service.Model;
using SharpNet.Service.Response;

namespace SharpNet.Service.Request
{
    [DataContract]
    [Route("/eval")]
    public class EvaluateRequest
        : IReturn<EvaluateResponse>
    {
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public Code Code { get; set; }
    }
}