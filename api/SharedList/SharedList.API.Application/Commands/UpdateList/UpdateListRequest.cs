using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Commands.UpdateList
{
    public class UpdateListRequest : IRequest
    {
        public ListDTO DTO { get; set; }
        public string UserIdent { get; set; }

        public UpdateListRequest(ListDTO dto, string userIdent)
        {
            DTO = dto;
            UserIdent = userIdent;
        }
    }
}