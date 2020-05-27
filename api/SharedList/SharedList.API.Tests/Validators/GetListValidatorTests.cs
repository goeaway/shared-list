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
            const string USER = "user";
            var request = new GetListRequest(null, USER);
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyId()
        {
            const string USER = "user";
            var request = new GetListRequest("", USER);
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetId()
        {
            const string USER = "user";
            var request = new GetListRequest("a", USER);
            var validator = new GetListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.Id, request);
        }

        [TestMethod]
        public void FailsOnNullUserId()
        {
            var request = new GetListRequest(null, null);
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserId not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyUserId()
        {
            var request = new GetListRequest("", "");
            var validator = new GetListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserId not set", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetUserId()
        {
            const string USER = "user";
            var request = new GetListRequest("a", USER);
            var validator = new GetListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.UserIdent, request);
        }
    }
}
