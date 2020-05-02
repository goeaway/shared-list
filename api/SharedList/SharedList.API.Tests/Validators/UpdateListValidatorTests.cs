using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - UpdateList")]
    public class UpdateListValidatorTests
    {
        [TestMethod]
        public void FailsWhenDTONull()
        {
            var request = new UpdateListRequest(null);
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdNull()
        {
            var request = new UpdateListRequest(new ListDTO());
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdEmpty()
        {
            var request = new UpdateListRequest(new ListDTO
            {
                Id = ""
            });
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesWhenIdSet()
        {
            var request = new UpdateListRequest(new ListDTO
            {
                Id = "a"
            });
            var validator = new UpdateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.DTO.Id, request);
        }
    }
}
