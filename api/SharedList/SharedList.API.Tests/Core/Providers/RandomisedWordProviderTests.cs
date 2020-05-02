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
        public void ReturnsAStringOfRandomWords()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateWordsString();

            Assert.AreEqual("EverlastingCavernousBlachindla", result);
        }

        [TestMethod]
        public void ReturnsADifferentStringEachCall()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateWordsString();
            var result2 = provider.CreateWordsString();

            Assert.AreNotEqual(result, result2);
        }

        [TestMethod]
        public void ReturnedStringShouldHaveThreeUpperCaseLetters()
        {
            var random = new Random(1);
            var provider = new RandomisedWordProvider(random);

            var result = provider.CreateWordsString();
            Assert.AreEqual(3, result.Count(r => char.IsUpper(r)));
        }
    }
}
