using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedList.API.Application.Commands.CreateList;
using SharedList.API.Application.Commands.DeleteList;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Application.Queries.GetList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : Controller
    {
        private readonly IMediator _mediator;

        public ListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<ListDTO> GetList(string id)
        {
            return _mediator.Send(new GetListRequest(id));
        }

        [HttpPost]
        public Task<string> CreateList(ListDTO dto)
        {
            return _mediator.Send(new CreateListRequest(dto));
        }

        [HttpPut]
        public Task UpdateList(ListDTO dto)
        {
            return _mediator.Send(new UpdateListRequest(dto));
        }

        [HttpDelete]
        public Task DeleteList(string id)
        {
            return _mediator.Send(new DeleteListRequest(id));
        }
    }
}