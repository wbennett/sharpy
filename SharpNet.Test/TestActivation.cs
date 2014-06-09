using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.ServiceModel;
using SharpNet.Service;
using SharpNet.Service.Model;
using SharpNet.Service.Request;

namespace SharpNet.Test
{
    [TestClass]
    public class TestIntegration
        :TestBase
    {

        [TestMethod]
        public void TestBasicActivationPlusCrossRequestBasicEvaluation()
        {
            //there should be no session
            var svc = new SharpyService();
            var resp = svc.Post(new EvaluateRequest()
            {
                Code = new Code()
                {
                    Value = @"Console.WriteLine(""hello world"");"
                }
            });

            //positive test
            Assert.IsNotNull(resp);

            if(resp.ResponseStatus != null)
                Assert.IsTrue(!resp.ResponseStatus.Errors.Any());

            Assert.IsTrue(resp.StandardOut.Trim() == "hello world");
            //there should be no session to start with...
            Assert.IsTrue(resp.SessionId != Guid.Empty.ToString());

            var prior = resp.SessionId;

            /*

               The following test will check cross request support.
            */
            resp = svc.Post(new EvaluateRequest()
            {
                Code = new Code()
                {
                    Value = @"var val=1;"
                },
                SessionId = resp.SessionId
            });

            Assert.IsTrue(resp.SessionId == prior);
            Assert.IsTrue(resp.ReturnValue == null);
            Assert.IsTrue(resp.StandardOut.IsNullOrEmpty());
            Assert.IsTrue(resp.StandardError.IsNullOrEmpty());

            prior = resp.SessionId;

            resp = svc.Post(new EvaluateRequest()
            {
                Code = new Code()
                {
                    Value = @"var res = val+1;res"
                },
                SessionId = resp.SessionId
            });

            Assert.IsTrue(resp.SessionId == prior);
            Assert.IsTrue(resp.StandardOut.IsNullOrEmpty());
            Assert.IsTrue(resp.StandardError.IsNullOrEmpty());
            Assert.IsTrue(int.Parse(resp.ReturnValue) == 2);

        }

        [TestMethod]
        public void TestMultiSessionMultiUser()
        {
            var svc = new SharpyService();
            var resp = svc.Post(new EvaluateRequest()
            {
                Code = new Code()
                {
                    Value = @"var two = 2;two"
                }
            });

            //positive test
            Assert.IsNotNull(resp);
            Assert.IsTrue(int.Parse(resp.ReturnValue) == 2);

            //make sure they don't collide

            resp = svc.Post(new EvaluateRequest()
            {
                Code =new Code()
                {
                    Value = @"two"
                }  
            });

            Assert.IsTrue(resp.ReturnValue.IsNullOrEmpty());
            Assert.IsTrue(!resp.StandardError.IsNullOrEmpty());

        }
    }
}