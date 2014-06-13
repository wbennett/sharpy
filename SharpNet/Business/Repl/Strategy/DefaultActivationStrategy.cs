using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
                var codebase = new Uri(Assembly.GetExecutingAssembly()
                    .CodeBase).LocalPath;
                Trace.TraceInformation(codebase);
                //get directory contents
                var fs = Directory.GetFiles(codebase);
                foreach (var s in fs)
                {
                    Trace.TraceInformation(s);
                }
                //apply default activation
                entity.Session.AddReference(
                    string.Format("{0}System.dll",codebase));
                entity.Session.AddReference(
                    string.Format("{0}System.Core.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Data.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Collections.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Collections.Concurrent.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Linq.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Runtime.Serialization.dll"));
                entity.Session.AddReference(
                    string.Format("{0}System.Xml"));
                entity.Session.AddReference(
                    string.Format("{0}System.Xml.Linq"));
                entity.Session.AddReference(
                    string.Format("{0}SharpNet.dll"));
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