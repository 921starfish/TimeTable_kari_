using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(15, sum(7, 8));
        }

        private int sum(int a,int b)
        {
            return a + b;
        }
    }
}
