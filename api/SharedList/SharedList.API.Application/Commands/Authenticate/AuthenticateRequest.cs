using MediatR;

namespace SharedList.API.Application.Commands.Authenticate
{
    public class AuthenticateRequest : IRequest<AuthenticateResponse>
    {
        public string IdToken { get; set; }

        public AuthenticateRequest(string idToken)
        {
            IdToken = idToken;
        }
    }
}