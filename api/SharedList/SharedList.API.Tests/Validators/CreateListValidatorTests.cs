using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.CreateList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Tests.Validators
{
    [TestClass]
    [TestCategory("Validators - CreateList")]
    public class CreateListValidatorTests
    {
        [TestMethod]
        public void FailsOnNullDTO()
        {
            var request = new CreateListRequest(null);
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnNullName()
        {
            var request = new CreateListRequest(new ListDTO());
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Name, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Name must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyName()
        {
            var request = new CreateListRequest(new ListDTO
            {
                Name = string.Empty
            });

            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Name, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Name must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetName()
        {
            var request = new CreateListRequest(new ListDTO
            {
                Name = "a"
            });

            var validator = new CreateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.DTO.Name, request);
        }
    }
}
