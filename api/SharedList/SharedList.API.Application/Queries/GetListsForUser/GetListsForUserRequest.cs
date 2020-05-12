using System.Collections.Generic;
using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Queries.GetListsForUser
{
    public class GetListsForUserRequest : IRequest<IEnumerable<ListDTO>>
    {
        public string UserIdent { get; set; }

        public GetListsForUserRequest(string userIdent)
        {
            UserIdent = userIdent;
        }
    }
}