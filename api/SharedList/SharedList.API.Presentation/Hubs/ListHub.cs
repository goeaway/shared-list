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
using SharedList.Core.Extensions;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Presentation.Hubs
{
    [Authorize]
    public class ListHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public ListHub(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
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
            var userIdent = _contextAccessor.GetUserIdent();

            await _mediator.Send(new UpdateListRequest(dto, userIdent));
            await Clients.Group(dto.Id).SendCoreAsync("UpdateList", new object[] {dto, userIdent});
        }
    }
}
