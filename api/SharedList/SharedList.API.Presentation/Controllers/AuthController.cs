using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SharedList.API.Application.Commands.Authenticate;

namespace SharedList.API.Presentation.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("authenticate")]
        public Task<AuthenticateResponse> Authenticate(string idToken)
        {
            return _mediator.Send(new AuthenticateRequest(idToken));
        }
    }
}