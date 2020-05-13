using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedList.API.Application.Commands.CreateList;
using SharedList.API.Application.Commands.DeleteList;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Application.Queries.GetListsForUser;
using SharedList.Core.Extensions;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ListController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public ListController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("getuserlists")]
        public Task<IEnumerable<ListDTO>> GetListsForUser()
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new GetListsForUserRequest(userIdent));
        }

        [HttpGet("get/{id}")]
        public Task<ListDTO> Get(string id)
        {
            return _mediator.Send(new GetListRequest(id));
        }

        [HttpPost("create")]
        public Task<string> Create(ListDTO dto)
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new CreateListRequest(dto, userIdent));
        }

        [HttpPut("update")]
        public Task Update(ListDTO dto)
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new UpdateListRequest(dto, userIdent));
        }

        [HttpDelete("delete")]
        public Task Delete(string id)
        {
            return _mediator.Send(new DeleteListRequest(id));
        }
    }
}