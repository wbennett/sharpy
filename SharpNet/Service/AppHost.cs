using ServiceStack.WebHost.Endpoints;

namespace SharpNet.Service
{
    public class AppHost
        : AppHostHttpListenerBase
    {

        //initialize the app host...
        public AppHost() 
            : base("Sharpy The CSharp Evaluator", typeof (SharpyService).Assembly)
        {
            
        }

        public override void Configure(Funq.Container container)
        {
            //configure the container...
            // it is configured upon first access
            SharpNet.Container.GetInstance();
        }
    }
}