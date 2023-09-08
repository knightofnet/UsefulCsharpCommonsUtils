using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefulCsharpCommonsUtils.lang;

namespace UnitTestProject1.lang.Test
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





        /// <summary>
        /// Teste si la méthode SubstringsByLen retourne les sous-chaînes correctes pour une chaîne donnée et une longueur de sous-chaîne donnée.
        /// </summary>
        [TestMethod]
        public void SubstringsByLen_ReturnsCorrectSubstrings()
        {
            // Arrange
            string str = "AbCdEfGhIjKlMnOp";
            int len = 2;
            string[] expected = { "Ab", "Cd", "Ef", "Gh", "Ij", "Kl", "Mn", "Op" };

            // Act
            string[] actual = CommonsStringUtils.SubstringsByLen(str, len);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Teste si la méthode SubstringsByLen retourne la chaîne d'origine lorsque la longueur de sous-chaîne est supérieure à la longueur de la chaîne.
        /// </summary>
        [TestMethod]
        public void SubstringsByLen_ReturnsOriginalString_WhenLengthIsGreaterThanStringLength()
        {
            // Arrange
            string str = "AbCdEfGhIjKlMnOp";
            int len = 20;
            string[] expected = { str };

            // Act
            string[] actual = CommonsStringUtils.SubstringsByLen(str, len);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Teste si la méthode SubstringsByLen retourne un tableau vide lorsque la chaîne est nulle.
        /// </summary>
        [TestMethod]
        public void SubstringsByLen_ReturnsEmptyArray_WhenStringIsNull()
        {
            // Arrange
            string str = null;
            int len = 2;
            string[] expected = { };

            // Act
            string[] actual = CommonsStringUtils.SubstringsByLen(str, len);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Teste si la méthode SubstringsByLen retourne un tableau vide lorsque la longueur de la chaîne est zéro.
        /// </summary>
        [TestMethod]
        public void SubstringsByLen_ReturnsEmptyArray_WhenStringLengthIsZero()
        {
            // Arrange
            string str = "";
            int len = 2;
            string[] expected = { };

            // Act
            string[] actual = CommonsStringUtils.SubstringsByLen(str, len);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Teste si la méthode SubstringsByLen retourne les sous-chaînes correctes pour une chaîne donnée et une longueur de sous-chaîne donnée, même lorsque la longueur de la chaîne n'est pas un multiple de la longueur de sous-chaîne.
        /// </summary>
        [TestMethod]
        public void SubstringsByLen_ReturnsCorrectSubstrings_WhenStringLengthIsNotMultipleOfLength()
        {
            // Arrange
            string str = "AbCdEfGhIjKlMnO";
            int len = 2;
            string[] expected = { "Ab", "Cd", "Ef", "Gh", "Ij", "Kl", "Mn", "O" };

            // Act
            string[] actual = CommonsStringUtils.SubstringsByLen(str, len);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }



    }
}