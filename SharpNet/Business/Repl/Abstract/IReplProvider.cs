using System;
using Roslyn.Scripting;

namespace SharpNet.Business.Repl.Abstract
{
    public interface IReplProvider
    {
        ReplContext GetContext(Guid identifier);
    }
}