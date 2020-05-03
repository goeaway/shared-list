using SharedList.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentValidation.Results;

namespace SharedList.API.Application.Exceptions
{
    public class RequestValidationFailedException : SharedListException
    {
        public IList<ValidationFailure> Failures { get; }
            = new List<ValidationFailure>();

        public RequestValidationFailedException(IList<ValidationFailure> failures)
        {
            Failures = failures;
        }

        public RequestValidationFailedException()
        {
        }

        public RequestValidationFailedException(string message) : base(message)
        {
        }

        public RequestValidationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
