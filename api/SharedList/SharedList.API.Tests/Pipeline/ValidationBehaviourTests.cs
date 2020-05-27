using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedList.API.Application.Exceptions;
using SharedList.API.Application.Pipeline;
using SharedList.API.Application.Queries.GetList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Tests.Pipeline
{
    [TestClass]
    [TestCategory("Pipeline - ValidationBehaviour")]
    public class ValidationBehaviourTests
    {
        [TestMethod]
        public async Task CallsNextIfNoValidationErrorsOccur()
        {
            const string LIST_ID = "id";
            const string USER = "user";

            var validators = new List<IValidator<GetListRequest>>
            {
                new GetListValidator()
            };

            var request = new GetListRequest(LIST_ID, USER);
            var validationBehaviour = new ValidationBehaviour<GetListRequest, ListDTO>(validators);
            var handlerMock = new Mock<RequestHandlerDelegate<ListDTO>>();
            var mockReturned = new ListDTO();
            handlerMock.Setup(m => m.Invoke()).ReturnsAsync(mockReturned);

            var result = await validationBehaviour.Handle(request, CancellationToken.None, handlerMock.Object);

            Assert.AreSame(mockReturned, result);
            handlerMock.Verify(m => m.Invoke(), Times.Once);
        }

        [TestMethod]
        public async Task ThrowsErrorIfAValidationErrorOccurs()
        {
            const string USER = "user";
            var validators = new List<IValidator<GetListRequest>>
            {
                new GetListValidator()
            };

            var request = new GetListRequest(string.Empty, USER);
            var validationBehaviour = new ValidationBehaviour<GetListRequest, ListDTO>(validators);

            var handlerMock = new Mock<RequestHandlerDelegate<ListDTO>>();

            await Assert.ThrowsExceptionAsync<RequestValidationFailedException>(
                () => validationBehaviour.Handle(request, CancellationToken.None, handlerMock.Object));
        }

        [TestMethod]
        public async Task ValidationErrorsAreAddedToException()
        {
            const string USER = "user";
            var validators = new List<IValidator<GetListRequest>>
            {
                new GetListValidator()
            };

            var request = new GetListRequest(string.Empty, USER);
            var validationBehaviour = new ValidationBehaviour<GetListRequest, ListDTO>(validators);

            var handlerMock = new Mock<RequestHandlerDelegate<ListDTO>>();

            try
            {
                await validationBehaviour.Handle(request, CancellationToken.None, handlerMock.Object);
                // an exception is expected, if we got here the test failed
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(RequestValidationFailedException));
                Assert.AreEqual(1, (e as RequestValidationFailedException).Failures.Count);
            }
        }
    }
}
