using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedList.API.Application.Commands.CreateEmptyList;
using SharedList.API.Application.Commands.DeleteList;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Application.Queries.GetListsForUser;
using SharedList.API.Application.Queries.GetName;
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

        [HttpGet("getlistpreviews")]
        public Task<IEnumerable<ListPreviewDTO>> GetListPreviews()
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new GetListPreviewsRequest(userIdent));
        }

        [HttpGet("getname")]
        [AllowAnonymous]
        public Task<string> GetName()
        {
            return _mediator.Send(new GetNameRequest());
        }

        [HttpGet("get/{id}")]
        public Task<ListDTO> Get(string id)
        {
            return _mediator.Send(new GetListRequest(id));
        }

        [HttpPost("createempty")]
        public Task<string> CreateEmpty()
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new CreateEmptyListRequest(userIdent));
        }

        [HttpPut("update")]
        public Task Update(ListDTO dto)
        {
            var userIdent = _contextAccessor.GetUserIdent();
            return _mediator.Send(new UpdateListRequest(dto, userIdent));
        }

        [HttpDelete("delete/{id}")]
        public Task Delete(string id)
        {
            return _mediator.Send(new DeleteListRequest(id));
        }
    }
}