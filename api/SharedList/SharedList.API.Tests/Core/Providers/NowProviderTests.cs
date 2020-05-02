using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.Core.Implementations;

namespace SharedList.API.Tests.Core.Providers
{
    [TestClass]
    [TestCategory("Providers - NowProvider")]
    public class NowProviderTests
    {
        [TestMethod]
        public void ReturnsASetDate()
        {
            var provider = new NowProvider();
            Assert.AreNotEqual(DateTime.MinValue, provider.Now);
        }

        [TestMethod]
        public void ReturnsTheCurrentDateTime()
        {
            var provider = new NowProvider();
            var realNow = DateTime.Now;
            var provided = provider.Now;
            // make sure they're at least pretty close
            Assert.IsTrue(Math.Abs(realNow.Ticks - provided.Ticks) < 1000);
        }
    }
}
