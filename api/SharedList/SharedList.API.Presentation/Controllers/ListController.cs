using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharedList.API.Application.Commands.CreateList;
using SharedList.API.Application.Commands.DeleteList;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Application.Queries.GetListName;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ListController : Controller
    {
        private readonly IMediator _mediator;

        public ListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getnames")]
        public Task<IEnumerable<ListNameAndIdDTO>> GetNames(string ids)
        {
            return _mediator.Send(new GetListNamesRequest(ids.Split(",")));
        }

        [HttpGet("get/{id}")]
        public Task<ListDTO> Get(string id)
        {
            return _mediator.Send(new GetListRequest(id));
        }

        [HttpPost("create")]
        public Task<string> Create(ListDTO dto)
        {
            return _mediator.Send(new CreateListRequest(dto));
        }

        [HttpPut("update")]
        public Task Update(ListDTO dto)
        {
            return _mediator.Send(new UpdateListRequest(dto));
        }

        [HttpDelete("delete")]
        public Task Delete(string id)
        {
            return _mediator.Send(new DeleteListRequest(id));
        }
    }
}