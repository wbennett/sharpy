using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Common;
using ServiceStack.ServiceClient.Web;
using SharpNet.Service.Model;
using SharpNet.Service.Request;
using SharpNet.Service.Response;

namespace SharpNet.Test
{
    [TestClass]
    public class TestSharpyServer
    {
        private const string Uri = "http://localhost:4000/";
        private void AssertHostExists()
        {

            var client = new WebClient();
            try
            {
                var resp = client.DownloadString(Uri);
                Assert.IsTrue(!resp.IsNullOrEmpty()); 
            }
            catch(Exception)
            {
                throw new AssertFailedException(
                    string.Format("Cannot reach the web server, for this test make sure you start sharpy on port the correct port. (./sharpy.exe {0})",
                    Uri));
            }
        }
        [TestMethod]
        public void TestHostExists()
        {
            AssertHostExists();
        }

        [TestMethod]
        public void TestBasicScript()
        {
            var rest = new JsonServiceClient(Uri);
            var resp = rest.Post(new EvaluateRequest()
            {
                Code = new Code()
                {
                    Value = "var val =1+1;val"
                } 
            });

            Assert.IsNotNull(resp);
            Assert.IsNull(resp.ResponseStatus);
            Assert.IsTrue(int.Parse(resp.ReturnValue) == 2);

            var id = resp.SessionId;
            resp = rest.Post(new EvaluateRequest()
            {
                SessionId = id,
                Code = new Code()
                {
                    Value = "val"
                } 
            });
            
            Assert.IsNotNull(resp);
            Assert.IsNull(resp.ResponseStatus);
            Assert.IsTrue(int.Parse(resp.ReturnValue) == 2);


        }
    }
}