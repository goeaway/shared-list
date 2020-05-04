using MediatR;
using System.Collections.Generic;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Queries.GetListName
{
    public class GetListNamesRequest : IRequest<IEnumerable<ListNameAndIdDTO>>
    {
        public string[] Ids { get; set; }

        public GetListNamesRequest(string[] ids)
        {
            Ids = ids;
        }
    }
}