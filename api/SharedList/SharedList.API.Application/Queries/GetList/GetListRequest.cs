using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Application.Queries.GetList
{
    public class GetListRequest : IRequest<ListDTO>
    {
        public string Id { get; set; }

        public GetListRequest(string id)
        {
            Id = id;
        }
    }
}
