using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Presentation.Models;
using SharedList.Core.Extensions;
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

        public async Task UpdateList(UpdateListHubDTO payload)
        {
            await _mediator.Send(new UpdateListRequest(payload.DTO, payload.UserIdent));
            await Clients.OthersInGroup(payload.DTO.Id).SendAsync("UpdateList", payload.DTO, payload.UserIdent);
        }
    }
}
