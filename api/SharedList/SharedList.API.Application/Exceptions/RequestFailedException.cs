using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using SharedList.Core.Exceptions;

namespace SharedList.API.Application.Exceptions
{
    public class RequestFailedException : SharedListException
    {
        public HttpStatusCode Code { get; set; }
            = HttpStatusCode.BadRequest;

        public RequestFailedException()
        {
        }

        public RequestFailedException(HttpStatusCode code)
        {
            Code = code;
        }

        public RequestFailedException(string message) : base(message)
        {
        }

        public RequestFailedException(string message, HttpStatusCode code) : base(message)
        {
            Code = code;
        }


        public RequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RequestFailedException(string message, HttpStatusCode code, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
