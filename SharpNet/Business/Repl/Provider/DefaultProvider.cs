using System;
using Ninject;
using Roslyn.Scripting;
using ServiceStack.CacheAccess;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Business.Repl.Factory;

namespace SharpNet.Business.Repl.Provider
{
    public class DefaultProvider
        : IReplProvider
    {
        public ReplContext GetContext(Guid identifier)
        {
            var client = Container.GetInstance()
                .Kernel.Get<ICacheClient>();
            var ctx = client
                .Get<ReplContext>(identifier.ToString());

            if (ctx != null)
                return ctx;

            //get a new instance use the bound activation strategy
            ctx = ReplContextFactory.NewInstance();

            //cache the context
            client.Add(ctx.Id.ToString(), ctx);

            return ctx;
        }
    }
}