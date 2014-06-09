using System;
using Ninject;
using Roslyn.Scripting.CSharp;
using SharpNet.Business.Repl.Abstract;

namespace SharpNet.Business.Repl.Factory
{
    public class ReplContextFactory
    {
        public static ReplContext NewInstance()
        {
            return NewInstance(Container.GetInstance()
                .Kernel.Get<IActivationStrategy<ReplContext>>());
        }

        public static ReplContext NewInstance(IActivationStrategy<ReplContext> strategy)
        {
            if(strategy == null)
                throw new InvalidOperationException("Activation strategy cannot be null.");

            var engine = Container.GetInstance()
                .Kernel.Get<ScriptEngine>();

            var ctx = new ReplContext()
            {
                Id = Guid.NewGuid(),
                Session = engine.CreateSession()
            };
            strategy.Activate(ctx);
            return ctx;
        }
    }
}