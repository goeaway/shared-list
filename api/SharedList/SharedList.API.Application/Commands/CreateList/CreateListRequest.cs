using MediatR;

namespace SharedList.API.Application.Commands.CreateList
{
    public class CreateListRequest : IRequest<string>
    {
        public string Name { get; set; }

        public CreateListRequest(string name)
        {
            Name = name;
        }
    }
}