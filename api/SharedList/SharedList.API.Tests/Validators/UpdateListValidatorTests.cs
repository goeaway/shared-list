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
        public void FailsWhenUserIdentNull()
        {
            var request = new UpdateListRequest(null, null);
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenUserIdentEmpty()
        {
            var request = new UpdateListRequest(null, "");
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.UserIdent, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("UserIdent must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenDTONull()
        {
            const string USER = "user";
            var request = new UpdateListRequest(null, USER);
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO must not be null", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdNull()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO(), USER);
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsWhenIdEmpty()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO
            {
                Id = ""
            }, USER);
            var validator = new UpdateListValidator();
            var failures = validator.ShouldHaveValidationErrorFor(r => r.DTO.Id, request);

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("DTO Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void FailsOnListItemDTONullId()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO()
                }
            }, USER);

            var validator = new UpdateListValidator();
            var result = validator.TestValidate(request);
            var failures = result.ShouldHaveValidationErrorFor("DTO.Items[0].Id");

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("List Item Id must not be empty", failures.First().ErrorMessage);
        }


        [TestMethod]
        public void FailsOnListItemDTONullEmpty()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO
                    {
                        Id = string.Empty
                    }
                }
            }, USER);

            var validator = new UpdateListValidator();
            var result = validator.TestValidate(request);
            var failures = result.ShouldHaveValidationErrorFor("DTO.Items[0].Id");

            Assert.AreEqual(1, failures.Count());
            Assert.AreEqual("List Item Id must not be empty", failures.First().ErrorMessage);
        }

        [TestMethod]
        public void PassesWhenIdSet()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO
            {
                Id = "a"
            }, USER);
            var validator = new UpdateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.DTO.Id, request);
        }

        [TestMethod]
        public void PassesOnSetItemId()
        {
            const string USER = "user";
            var request = new UpdateListRequest(new ListDTO
            {
                Items = new List<ListItemDTO>
                {
                    new ListItemDTO
                    {
                        Id = "a"
                    }
                }
            }, USER);

            var validator = new UpdateListValidator();
            var result = validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor("DTO.Items[0].Id");
        }

        [TestMethod]
        public void PassesOnSetUserIdent()
        {
            const string USER = "user";
            var request = new UpdateListRequest(null, USER);

            var validator = new UpdateListValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.UserIdent, request);
        }
    }
}
