using System; using System.Dynamic;
using System.Web.Script.Serialization;
using Ninject;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Service.Request;
using SharpNet.Service.Response;

namespace SharpNet.Business.Repl.Strategy
{
    public class EvaluateRequestStrategy
        : IStrategy
    {
        public EvaluateRequestStrategy(
            EvaluateRequest request,
            Action<EvaluateResponse> responseHandler 
            )
        {
            this.Request = request;
            this.ResponseHandler = responseHandler;
        }

        private Action<EvaluateResponse> ResponseHandler { get; set; }

        private EvaluateRequest Request { get; set; }

        public void Execute()
        {
            if(Request == null)
                throw new InvalidOperationException(
                    "Cannot evaluate request without request."
                    );

            dynamic resp = new ExpandoObject();
            //execute the users code
            var provider = Container.GetInstance()
                .Kernel.Get<IReplProvider>();

            var context = provider.GetContext(Guid.Parse(Request.SessionId));
            resp.SessionId = context.Id.ToString();
            resp.Result = default(object);
            resp.StandardError = default(string);
            resp.StandardOut = default(string);
            try
            {
                //execute the code
                resp.Result = context.Session.Execute(this.Request.Code.Value);
                //grab the standard out
                resp.StandardOut = context.Session
                    .Execute<string>(
                    string.Format("SharpNet.System.ConsoleBuffer.Instance(\"{0}\").FlushOut();",
                          context.Id)
                    );
                //grab the standard error
                resp.StandardError = context.Session
                    .Execute<string>(
                        string.Format("SharpNet.System.ConsoleBuffer.Instance(\"{0}\").FlushError();",
                            context.Id)
                    );

            }
            catch (Exception e)
            {
                resp.StandardError = e.Message;
            }

            if (ResponseHandler == null)
                return;

            //try to serialize the return value...
            string resultStr = null;
            if (resp.Result != null)
                resultStr = new JavaScriptSerializer().Serialize(resp.Result);

            ResponseHandler(new EvaluateResponse()
            {
               //get out 
               ReturnValue = resultStr,
               StandardError = resp.StandardError,
               StandardOut = resp.StandardOut,
               SessionId = resp.SessionId
            });

        }
    }
}