using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.API.Application.Exceptions;
using SharedList.Core.Abstractions;
using SharedList.Persistence;

namespace SharedList.API.Application.Commands.UpdateListItem
{
    public class UpdateListItemHandler : IRequestHandler<UpdateListItemRequest>
    {
        private readonly SharedListContext _context;
        private readonly INowProvider _nowProvider;

        public UpdateListItemHandler(SharedListContext context, INowProvider nowProvider)
        {
            _context = context;
            _nowProvider = nowProvider;
        }

        public async Task<Unit> Handle(UpdateListItemRequest request, CancellationToken cancellationToken)
        {
            var existing = await _context.ListItems.FindAsync(request.DTO.Id);

            // it's possible someone else deleted the thing we're editing
            if (existing != null)
            {
                existing.Value = request.DTO.Value;
                existing.Notes = request.DTO.Notes;
                existing.Completed = request.DTO.Completed;
                existing.Updated = _nowProvider.Now;

                await _context.SaveChangesAsync();
            }

            return Unit.Value;
        }
    }
}
