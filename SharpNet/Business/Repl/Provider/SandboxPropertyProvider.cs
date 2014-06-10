using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNet.Business.Repl.Provider
{
    public class SandboxPropertyProvider
    {
        public SandboxPropertyProvider(TimeSpan maxtime)
        {
            this.MaximumExecutionTimeSpan = maxtime;
        }
        public TimeSpan MaximumExecutionTimeSpan { get;private set; }
    }
}
