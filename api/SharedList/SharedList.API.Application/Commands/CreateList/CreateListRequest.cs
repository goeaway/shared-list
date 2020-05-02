using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Commands.CreateList
{
    public class CreateListRequest : IRequest<string>
    {
        public ListDTO DTO { get; set; }

        public CreateListRequest(ListDTO dto)
        {
            DTO = dto;
        }
    }
}