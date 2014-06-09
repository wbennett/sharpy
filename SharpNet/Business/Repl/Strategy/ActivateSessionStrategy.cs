using System;
using Ninject;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Service.Response;

namespace SharpNet.Business.Repl.Strategy
{
    public class ActivateSessionStrategy
        : IStrategy
    {
        public Action<ActivateResponse> Handler { get; set; }

        public ActivateSessionStrategy(
            Action<ActivateResponse> handler
            )
        {
            this.Handler = handler;
        }

        public void Execute()
        {

            var provider = Container.GetInstance()
                .Kernel.Get<IReplProvider>();

            var context = provider.GetContext(Guid.Empty);

            
            if(Handler == null)
                throw new InvalidOperationException(
                    "Activate handler is null");

            Handler(new ActivateResponse()
            {
                SessionId = context.Id.ToString()
            });
        }
    }
}