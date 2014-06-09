using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting.CSharp;

namespace SharpNet.Test
{
    [TestClass]
    public class TestBasicEvaluation
    {
        [TestMethod]
        public void TestBasicEval()
        {
            var tree = Syntax.ParseCompilationUnit(
                @"var name = ""joe""; 
                    "
                );

            Assert.IsNotNull(tree);
            var engine = new ScriptEngine();
            var session = engine.CreateSession();
            session.AddReference("System.dll");
            session.AddReference("System.Linq.dll");
            session.AddReference("SharpNet.dll");
            session.ImportNamespace("System");
            session.ImportNamespace("System.Linq");
            session.ImportNamespace("SharpNet");
            var rval = session.Execute(@"var name = ""joe"";");
            rval = session.Execute<object>("name");
            var asStr = rval as string;
            Assert.IsTrue(asStr == "joe");
        }
    }
}
