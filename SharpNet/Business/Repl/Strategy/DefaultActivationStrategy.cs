using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NLog;
using SharpNet.Business.Repl.Abstract;
using SharpNet.Service.Model;

namespace SharpNet.Business.Repl.Strategy
{
    public class DefaultActivationStrategy
        : IActivationStrategy<ReplContext>
    {
        public void Activate(ReplContext entity)
        {
            try
            {
                var codebase = new FileInfo(new Uri(Assembly.GetExecutingAssembly()
                    .CodeBase).LocalPath).DirectoryName;
                codebase = codebase.EndsWith("\\")
                    ? codebase
                    : string.Format(@"{0}\", codebase);
                Trace.TraceInformation(codebase);
                //get directory contents
                var fs = Directory.GetFiles(codebase);

                foreach (var s in fs.Where(x=>x.ToLower()
                    .EndsWith(".dll")))
                {
                    entity.Session.AddReference(s);
                    Trace.TraceInformation(string.Format("reference added:{0}", s));
                }

                //find system core
                foreach (var s in AppDomain.CurrentDomain
                    .GetAssemblies().Where(x => x.FullName.Contains("System.Core"))
                    )
                {
                   entity.Session.AddReference(s); 
                    Trace.TraceInformation(string.Format("reference added:{0}", s));
                }
                /*
                //apply default activation
                entity.Session.AddReference(
                    string.Format("{0}System.dll",codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Core.dll",codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Data.dll", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Collections.dll", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Collections.Concurrent.dll", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Linq.dll", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Runtime.Serialization.dll", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Xml", codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Xml.Linq", codebase));
                entity.Session.AddReference(
                    string.Format("{0}SharpNet.dll", codebase));
                    */
                entity.Session.ImportNamespace("System");
                entity.Session.ImportNamespace("System.Collections.Generic");
                entity.Session.ImportNamespace("System.Linq");
                entity.Session.ImportNamespace("System.Text");
                entity.Session.ImportNamespace("System.Threading.Tasks");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
                LogManager.GetCurrentClassLogger()
                    .Error(e.Message);
                LogManager.GetCurrentClassLogger()
                    .Error(e.StackTrace);
                throw;
            }

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