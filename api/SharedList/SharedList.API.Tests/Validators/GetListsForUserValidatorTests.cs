using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Queries.GetListsForUser;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - GetListsForUser")]
    public class GetListsForUserValidatorTests
    {
        [TestMethod]
        public void FailsOnEmptyUserIdent()
        {
            var request = new GetListsForUserRequest("");
            var validator = new GetListsForUserValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnNullUserIdent()
        {
            var request = new GetListsForUserRequest(null);
            var validator = new GetListsForUserValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetUserIdent()
        {
            var request = new GetListsForUserRequest("user");
            var validator = new GetListsForUserValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.UserIdent, request);
        }
    }
}
