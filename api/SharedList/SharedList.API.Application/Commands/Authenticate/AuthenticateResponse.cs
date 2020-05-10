using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.API.Application.Commands.Authenticate
{
    public class AuthenticateResponse
    {
        public string JWT { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
