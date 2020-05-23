using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Commands.CreateEmptyList
{
    public class CreateEmptyListRequest : IRequest<string>
    {
        public string UserIdent { get; set; }

        public CreateEmptyListRequest(string userIdent)
        {
            UserIdent = userIdent;
        }
    }
}