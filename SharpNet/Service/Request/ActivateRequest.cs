using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using SharpNet.Service.Response;

namespace SharpNet.Service.Request
{
    [DataContract]
    [Route("/activate")]
    public class ActivateRequest
        : IReturn<ActivateResponse>
    {
        //this is an empty request
    }
}