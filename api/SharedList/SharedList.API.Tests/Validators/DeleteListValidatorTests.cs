using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.DeleteList;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - DeleteList")]
    public class DeleteListValidatorTests
    {
        [TestMethod]
        public void FailsWhenIdIsNull()
        {
            var request = new DeleteListRequest(null);
            var validator = new DeleteListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdIsEmpty()
        {
            var request = new DeleteListRequest("");
            var validator = new DeleteListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesWhenIdIsSet()
        {
            var request = new DeleteListRequest("a");
            var validator = new DeleteListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.Id, request);
        }
    }
}
