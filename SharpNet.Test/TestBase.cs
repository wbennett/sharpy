using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpNet.Service;

namespace SharpNet.Test
{
    [TestClass]
    public class TestBase
    {
        public AppHost AppHost { get;set; }

        public TestBase()
        {
            this.AppHost = new AppHost();
            //configure everything
            this.AppHost.Configure(null);
        }

        [TestInitialize]
        public void SetUp()
        {
            
        }
    }
}
