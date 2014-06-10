using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using SharpNet.Service;

namespace sharpy
{
    class Program
    {
                
        class Options
        {
            [Option('s',"server",Required = true,
                HelpText = "The base URI where the host should run. (ie http://localhost:4000)"
                )]
            public string Host { get; set; }
            [Option('v',"verbose", Required = false, DefaultValue = true)]
            public bool Verbose { get; set; }

            [HelpOption()]
            public string GetUsage()
            {

                var help = new HelpText()
                {
                    Heading = new HeadingInfo("Sharpy", "1.0"),
                    Copyright = "Paul Bennett (wm.paul.bennett@gmail.com)",
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true
                };
                help.AddPreOptionsLine("--------------------------------");
                help.AddOptions(this);
                return help;
            }
        }

        static void StartWebServer(Options options)
        {
            var appHost = new AppHost();
            appHost.Init();
            Console.WriteLine(string.Format(
                "Starting sharpy on {0}",
                options.Host
                ));
            appHost.Start(options.Host);
            
        }

        static void Main(string[] args)
        {
            var options = new Options();

            //handle the command line input
            if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
                return;

            //create a cancellation token source to send messages
            var tokenSource = new CancellationTokenSource();
            var tsk = Task.Factory.StartNew(
                () =>
                {

                    //run until we are told to stop
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            //start the web server
                            StartWebServer(options);
                            //wait until we are told to stop
                            while (!tokenSource.Token.IsCancellationRequested)
                            {
                            }
                        }
                        catch (Exception e)
                        {
                            //log the error and restart the server
                            Console.Error.WriteLine("{0}{1}{1}{2}{1}", 
                                e.Message, 
                                Environment.NewLine, 
                                e.StackTrace); 
                        }
                    }
                },tokenSource.Token);
            //run until we cancel
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
            tokenSource.Cancel();
            tsk.Wait();
        }
    }
}
