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
        public void FailsOnNullUserIdent()
        {
            var request = new CreateListRequest(null, null);
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyUserIdent()
        {
            var request = new CreateListRequest(null, "");
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnNullDTO()
        {
            const string USER = "user";
            var request = new CreateListRequest(null, USER);
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnNullName()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO(), USER);
            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Name, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Name must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnEmptyName()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO
            {
                Name = string.Empty
            }, USER);

            var validator = new CreateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Name, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("Name must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnListItemDTONullId()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO()
                }
            }, USER);

            var validator = new CreateListValidator();
            var result = validator.TestValidate(request);
            var failures = result.ShouldHaveValidationErrorFor("DTO.Items[0].Id");

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("List Item Id must not be empty", failures.First().ErrorMessage);
        }


        [TestMethod]
        public void FailsOnListItemDTONullEmpty()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO
                    {
                        Id = string.Empty
                    }
                }
            }, USER);

            var validator = new CreateListValidator();
            var result = validator.TestValidate(request);
            var failures = result.ShouldHaveValidationErrorFor("DTO.Items[0].Id");

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("List Item Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesOnSetName()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO
            {
                Name = "a"
            }, USER);

            var validator = new CreateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.DTO.Name, request);
        }

        [TestMethod]
        public void PassesOnSetUserIdent()
        {
            const string USER = "user";
            var request = new CreateListRequest(null, USER);

            var validator = new CreateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.UserIdent, request);
        }

        [TestMethod]
        public void PassesOnSetItemId()
        {
            const string USER = "user";
            var request = new CreateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO
                    {
                        Id = "a"
                    }
                }
            }, USER);

            var validator = new CreateListValidator();
            var result = validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor("DTO.Items[0].Id");
        }
    }
}
