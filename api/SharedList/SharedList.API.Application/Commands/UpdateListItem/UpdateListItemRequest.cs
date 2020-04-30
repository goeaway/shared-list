using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Commands.UpdateListItem
{
    public class UpdateListItemRequest : IRequest
    {
        public ListItemDTO DTO { get; set; }

        public UpdateListItemRequest(ListItemDTO dto)
        {
            DTO = dto;
        }
    }
}