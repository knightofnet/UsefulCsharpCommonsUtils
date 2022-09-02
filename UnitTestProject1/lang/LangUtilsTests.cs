using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefulCsharpCommonsUtils.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang.Tests
{
    [TestClass()]
    public class LangUtilsTests
    {
        [TestMethod()]
        public void IsEqWithNullTest()
        {
            string a = "a";
            string b = "b";

            Assert.IsTrue(b != a);
            Assert.IsTrue(!b.Equals(a));
            Assert.IsTrue(!LangUtils.IsEqWithNull(a, b));
            
            b = "a";
            Assert.IsTrue(b == a);
            Assert.IsTrue(b.Equals(a));
            Assert.IsTrue(LangUtils.IsEqWithNull(a, b));
            
            a = null;
            Assert.IsTrue(b != a);
            Assert.IsTrue(!b.Equals(a));
            Assert.IsTrue(!LangUtils.IsEqWithNull(a, b));

            Assert.IsTrue(a != b);
            Assert.ThrowsException<NullReferenceException>(() => !a.Equals(b));
            Assert.IsTrue(!LangUtils.IsEqWithNull(a, b));
        }
    }
}