using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefulCsharpCommonsUtils.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.lang.Tests
{
    [TestClass()]
    public class CommonsStringUtilsTests
    {

        [TestMethod()]
        public void SafeSubstringTest()
        {
            string str = "AbCdEfGhIjKlMnOp";

            string part = CommonsStringUtils.SafeSubstring(str, 0, str.Length);
            Assert.AreEqual(str, part);

            part = CommonsStringUtils.SafeSubstring(str, 0, 4);
            Assert.AreEqual("AbCd", part);

            str = "AbCd";
            part = CommonsStringUtils.SafeSubstring(str, 3, 2);
            Assert.AreEqual("d", part);

        }

        [TestMethod()]
        public void SubstringsByLenTest()
        {
            string str = "AbCdEfGhIjKlMnOp";

            string[] parts = CommonsStringUtils.SubstringsByLen(str, 2);

            Assert.AreEqual(8, parts.Length);
            Assert.AreEqual(parts[0], "Ab");
            Assert.AreEqual(parts[1], "Cd");
            Assert.AreEqual(parts[7], "Op");

            str = "AbCdEfGhIjKlMnOpQ";

            parts = CommonsStringUtils.SubstringsByLen(str, 2);

            Assert.AreEqual(9, parts.Length);
            Assert.AreEqual(parts[8], "Q");
        }

        [TestMethod()]
        public void RemoveCharDifferentThanTest()
        {
            String str = "Abcdefghijklmnop";

            String retStr = CommonsStringUtils.RemoveCharDifferentThan(str, "bh");
            Assert.AreEqual("bh", retStr);

            str = "Abbhhbbcdefghbhijklmnop";
            retStr = CommonsStringUtils.RemoveCharDifferentThan(str, "bh");
            Assert.AreEqual("bbhhbbhbh", retStr);
        }

        [TestMethod()]
        public void RemoveCharTest()
        {
            String str = "Abcdefghijklmnop";

            String retStr = CommonsStringUtils.RemoveChar(str, "bh");
            Assert.AreEqual("Acdefgijklmnop", retStr);

            str = "Abbhhbbcdefghbhijklmnop";
            retStr = CommonsStringUtils.RemoveChar(str, "bh");
            Assert.AreEqual("Acdefgijklmnop", retStr);
        }
    }
}