using System;
using System.Collections.Generic;
using Roslyn.Scripting;
using SharpNet.System;

namespace SharpNet.Business.Repl
{
    public class ReplContext
    {
        public Guid Id { get; set; }
        public Session Session { get; set; }
        public ConsoleBuffer Buffer { get; set; }
    }
}