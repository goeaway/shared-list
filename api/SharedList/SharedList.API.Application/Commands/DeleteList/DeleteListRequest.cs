using MediatR;

namespace SharedList.API.Application.Commands.DeleteList
{
    public class DeleteListRequest : IRequest
    {
        public string Id { get; set; }

        public DeleteListRequest(string id)
        {
            Id = id;
        }
    }
}