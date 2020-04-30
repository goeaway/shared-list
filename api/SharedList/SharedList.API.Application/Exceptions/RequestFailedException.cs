using System;
using System.Collections.Generic;
using System.Text;
using SharedList.Core.Exceptions;

namespace SharedList.API.Application.Exceptions
{
    public class RequestFailedException : SharedListException
    {
        public RequestFailedException()
        {
        }

        public RequestFailedException(string message) : base(message)
        {
        }

        public RequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
