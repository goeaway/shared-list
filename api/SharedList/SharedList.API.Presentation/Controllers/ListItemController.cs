using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedList.API.Application.Commands.DeleteListItem;
using SharedList.API.Application.Commands.UpdateListItem;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListItemController : Controller
    {
        private readonly IMediator _mediator;

        public ListItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public Task UpdateListItem(ListItemDTO dto)
        {
            return _mediator.Send(new UpdateListItemRequest(dto));
        }

        [HttpDelete]
        public Task DeleteListItem(int id)
        {
            return _mediator.Send(new DeleteListItemRequest(id));
        }
    }
}