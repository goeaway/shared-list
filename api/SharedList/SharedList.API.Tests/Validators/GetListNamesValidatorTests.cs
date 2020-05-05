using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation.TestHelper;
using SharedList.API.Application.Queries.GetListName;
using SharedList.API.Application.Queries.GetListNames;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - GetListNames")]
    public class GetListNamesValidatorTests
    {
        [TestMethod]
        public void FailsOnNullIds()
        {
            var request = new GetListNamesRequest(null);
            var validator = new GetListNamesValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Ids, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Must have at least one id", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyIds()
        {
            var request = new GetListNamesRequest(new string[]{});
            var validator = new GetListNamesValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Ids, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Must have at least one id", failures.First().ErrorMessage);
        }


        [TestMethod]
        public void PassesWithAtLeastOneId()
        {
            var request = new GetListNamesRequest(new string[] { "id" });
            var validator = new GetListNamesValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.Ids, request);
        }
    }
}
