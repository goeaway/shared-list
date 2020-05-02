using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.DeleteListItem;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - DeleteListItem")]
    public class DeleteListItemValidatorTests
    {
        [TestMethod]
        public void FailsWhenIdIsLessThanOne()
        {
            var request = new DeleteListItemRequest(0);
            var validator = new DeleteListItemValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Id must be greater than 0", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesWhenIdIsGreaterThanZero()
        {
            var request = new DeleteListItemRequest(1);
            var validator = new DeleteListItemValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.Id, request);
        }
    }
}
