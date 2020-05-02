using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Queries.GetList;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - GetList")]
    public class GetListValidatorTests
    {
        [TestMethod]
        public void FailsOnNullId()
        {
            var request = new GetListRequest(null);
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyId()
        {
            var request = new GetListRequest("");
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetId()
        {
            var request = new GetListRequest("a");
            var validator = new GetListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.Id, request);
        }
    }
}
