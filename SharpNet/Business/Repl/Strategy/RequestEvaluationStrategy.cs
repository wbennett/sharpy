using System; using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Ninject;
using Roslyn.Scripting;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Business.Repl.Provider;
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

        private string flushStandardOut(Session session,Guid id)
        {
            return session
                .Execute<string>(
                    string.Format("SharpNet.System.ConsoleBuffer.Instance(\"{0}\").FlushOut();",
                        id));
        }

        private string flushStandardError(Session session,Guid id)
        {
           return session 
                    .Execute<string>(
                        string.Format("SharpNet.System.ConsoleBuffer.Instance(\"{0}\").FlushError();",
                            id)
                    );
        }

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
            Exception threadException = null;
            try
            {
                context.Session.Engine.BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //execute the code
                var cts = new CancellationTokenSource();
                var task = Task.Factory.StartNew(state =>
                {
                    var token = (CancellationToken) state;
                    try
                    {
                        resp.Result = context.Session.Execute(this.Request.Code.Value);

                    }
                    catch (Exception e)
                    {
                        threadException = e;
                        throw e;
                    }
                    token.ThrowIfCancellationRequested();
                }, cts.Token);
                //wait maximum amount of time before blowing up
                if (!task.Wait((int) Container.GetInstance().Kernel.Get<SandboxPropertyProvider>()
                    .MaximumExecutionTimeSpan.TotalMilliseconds, cts.Token))
                {
                    cts.Cancel();
                    //clean up...
                    throw new InvalidOperationException("Command took too long to execute.");
                }
            }
            catch (InvalidOperationException e)
            {
                resp.StandardError = e.Message;
            }
            catch (Exception e)
            {
                resp.StandardError = threadException == null ? 
                    e.Message : threadException.Message;
            }

            resp.StandardOut = flushStandardOut(context.Session,context.Id);
            resp.StandardError = flushStandardError(context.Session,context.Id);

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