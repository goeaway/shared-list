using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedList.API.Application.Exceptions;
using SharedList.Core.Abstractions;

namespace SharedList.API.Application.Commands.Authenticate
{
    public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly INowProvider _nowProvider;

        public AuthenticateHandler(IConfiguration configuration, INowProvider nowProvider)
        {
            _configuration = configuration;
            _nowProvider = nowProvider;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, result.Name),
                        new Claim(ClaimTypes.Email, result.Email), 
                    }),
                    Expires = _nowProvider.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };

                // return ok with JWT
                var token = handler.CreateToken(tokenDescriptor);
                return new AuthenticateResponse
                {
                    JWT = handler.WriteToken(token),
                    Email = result.Email,
                    Name = result.Name
                };
            }
            catch (InvalidJwtException e)
            {
                throw new RequestFailedException("Authentication failed", HttpStatusCode.BadRequest, e);
            }
        }
    }
}
