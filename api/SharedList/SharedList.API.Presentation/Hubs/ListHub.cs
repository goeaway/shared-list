using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Hubs
{
    [Authorize]
    public class ListHub : Hub
    {
        private readonly IMediator _mediator;

        public ListHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task JoinList(string listId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, listId);
        }

        public Task LeaveList(string listId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, listId);
        }

        public async Task UpdateList(ListDTO dto)
        {
            await _mediator.Send(new UpdateListRequest(dto));
            await Clients.Group(dto.Id).SendCoreAsync("UpdateList", new[] {dto});
        }
    }
}
