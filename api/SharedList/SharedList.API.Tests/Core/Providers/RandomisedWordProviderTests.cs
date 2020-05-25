using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.Core.Implementations;

namespace SharedList.API.Tests.Core.Providers
{
    [TestClass]
    [TestCategory("Providers - RandomisedWordProvider")]
    public class RandomisedWordProviderTests
    {
        [TestMethod]
        public void RandomId_ReturnsAStringOfRandomWords()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateRandomId();

            Assert.AreEqual("EverlastingCavernousBougatsa", result);
        }

        [TestMethod]
        public void RandomId_ReturnsADifferentStringEachCall()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateRandomId();
            var result2 = provider.CreateRandomId();

            Assert.AreNotEqual(result, result2);
        }

        [TestMethod]
        public void RandomId_ReturnedStringShouldHaveThreeUpperCaseLetters()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateRandomId();
            Assert.AreEqual(3, result.Count(r => char.IsUpper(r)));
        }

        [TestMethod]
        public void RandomName_ReturnsAString()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);
            var result = provider.CreateRandomName();
            Assert.AreEqual("Everlasting Charlotte", result);
        }

        [TestMethod]
        public void RandomName_ReturnsDifferentStringEachCall()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);
            var result = provider.CreateRandomName();
            var result2 = provider.CreateRandomName();

            Assert.AreNotEqual(result, result2);
        }

        [TestMethod]
        public void RandomName_ReturnedValueHasASpaceInIt()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);
            var result = provider.CreateRandomName();
            Assert.IsTrue(result.Contains(" "));
        }

        [TestMethod]
        public void RandomName_ReturnedValueHasTwoUpperCaseCharsInIt()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);
            var result = provider.CreateRandomName();
            Assert.AreEqual(2, result.Count(r => char.IsUpper(r)));
        }
    }
}