using System.Collections.Generic;
using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Queries.GetListsForUser
{
    public class GetListPreviewsRequest : IRequest<IEnumerable<ListPreviewDTO>>
    {
        public string UserIdent { get; set; }

        public GetListPreviewsRequest(string userIdent)
        {
            UserIdent = userIdent;
        }
    }
}