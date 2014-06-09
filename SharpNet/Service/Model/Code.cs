using System.Runtime.Serialization;

namespace SharpNet.Service.Model
{
    [DataContract]
    public class Code
    {
        [DataMember]
        public string Value { get; set; } 
    }
}