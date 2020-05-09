using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Hubs
{
    public class ListHub : Hub
    {
        private readonly IMediator _mediator;

        public ListHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task UpdateList(ListDTO dto)
        {
            await _mediator.Send(new UpdateListRequest(dto));
            await Clients.All.SendCoreAsync("UpdateList", new [] { dto });
        }
    }
}
