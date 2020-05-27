using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Queries.GetList
{
    public class GetListRequest : IRequest<ListDTO>
    {
        public string UserIdent { get; set; }
        public string Id { get; set; }

        public GetListRequest(string id, string userIdent)
        {
            Id = id;
            UserIdent = userIdent;
        }
    }
}
