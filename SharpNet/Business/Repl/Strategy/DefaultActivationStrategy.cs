using System.IO;
using System.Reflection;
using SharpNet.Business.Repl.Abstract;

namespace SharpNet.Business.Repl.Strategy
{
    public class DefaultActivationStrategy
        : IActivationStrategy<ReplContext>
    {
        public void Activate(ReplContext entity)
        {
            //apply default activation
            entity.Session.AddReference("System.dll");
            entity.Session.AddReference("System.Core.dll");
            entity.Session.AddReference("System.Data.dll");
            entity.Session.AddReference("System.Collections.dll");
            entity.Session.AddReference("System.Collections.Concurrent.dll");
            entity.Session.AddReference("System.Linq.dll");
            entity.Session.AddReference("System.Runtime.Serialization.dll");
            entity.Session.AddReference("System.Xml");
            entity.Session.AddReference("System.Xml.Linq");
            entity.Session.AddReference("SharpNet.dll");
            entity.Session.ImportNamespace("System");
            entity.Session.ImportNamespace("System.Collections.Generic");
            entity.Session.ImportNamespace("System.Linq");
            entity.Session.ImportNamespace("System.Text");
            entity.Session.ImportNamespace("System.Threading.Tasks");

            try
            {
                //activate the current console
                var code = string.Format(
                    "SharpNet.System.ConsoleBuffer.Activate(\"{0}\");",
                    entity.Id
                    );
                entity.Session.Execute(code);
            }
            catch
            { }
        }
    }
}