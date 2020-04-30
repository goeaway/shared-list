using MediatR;

namespace SharedList.API.Application.Commands.DeleteListItem
{
    public class DeleteListItemRequest : IRequest
    {
        public int Id { get; set; }

        public DeleteListItemRequest(int id)
        {
            Id = id;
        }
    }
}