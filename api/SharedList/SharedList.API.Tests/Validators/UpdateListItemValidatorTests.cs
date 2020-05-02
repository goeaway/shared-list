using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.UpdateListItem;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - UpdateListItem")]
    public class UpdateListItemValidatorTests
    {
        [TestMethod]
        public void FailsWhenDTOIsNull()
        {
            var request = new UpdateListItemRequest(null);
            var validator = new UpdateListItemValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdIsLessThanOne()
        {
            var request = new UpdateListItemRequest(new ListItemDTO
            {
                Id = 0
            });
            var validator = new UpdateListItemValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO Id must be greater than 0", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesWhenIdIsGreaterThanZero()
        {
            var request = new UpdateListItemRequest(new ListItemDTO
            {
                Id = 1
            });
            var validator = new UpdateListItemValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.DTO.Id, request);
        }
    }
}
