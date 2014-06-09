using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        static void Main(string[] args)
        {
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
                return;

            var appHost = new AppHost();
            appHost.Init();
            Console.WriteLine(string.Format(
                "Starting sharpy on {0}",
                options.Host
                ));
            appHost.Start(options.Host);
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}
