using Ninject;
using Roslyn.Scripting.CSharp;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using SharpNet.Abstract;
using SharpNet.Business.Repl;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Business.Repl.Provider;
using SharpNet.Business.Repl.Strategy;

namespace SharpNet
{
    /// <summary>
    /// An app domain singleton to facilitate dependency injection
    /// </summary>
    public sealed class Container
        : IProvideKernel
    {

        private static Container _internalContainer = null;
        private static object lockobj = new object();

        private Container()
        {
            this.Kernel = new StandardKernel(); 
            RegisterDefaultServices(this.Kernel);
        }

        public static Container GetInstance()
        {
            if (_internalContainer != null)
                return _internalContainer;

            lock (lockobj)
            {
                if (_internalContainer != null)
                    return _internalContainer;

                return _internalContainer = new Container();
            }
        }

        public IKernel Kernel { get; private set; }


        /// <summary>
        /// This function is where default registration will occur
        /// </summary>
        private static void RegisterDefaultServices(IKernel kernel)
        {
            //register cache client
            kernel.Bind<ICacheClient>()
                .ToConstant(
                    new MemoryCacheClient()
                );
            
            //register the scripting engine
            kernel.Bind<ScriptEngine>()
                .ToConstant(
                    new ScriptEngine()
                );

            //register the strategy to activate
            kernel.Bind<IActivationStrategy<ReplContext>>()
                .ToConstant(
                    new DefaultActivationStrategy()
                );

            //register provider
            kernel.Bind<IReplProvider>()
                .ToConstant(
                    new DefaultProvider()
                );

        }
    }
}