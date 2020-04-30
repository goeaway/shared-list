using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Exceptions
{
    public class SharedListException : ApplicationException
    {
        public SharedListException() { }
        public SharedListException(string message) : base(message) { }
        public SharedListException(string message, Exception innerException) : base(message, innerException) { }
    }
}
