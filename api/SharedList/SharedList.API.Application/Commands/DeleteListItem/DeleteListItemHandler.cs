using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Persistence;

namespace SharedList.API.Application.Commands.DeleteListItem
{
    public class DeleteListItemHandler : IRequestHandler<DeleteListItemRequest>
    {
        private readonly SharedListContext _context;

        public DeleteListItemHandler(SharedListContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteListItemRequest request, CancellationToken cancellationToken)
        {
            var existing = await _context.ListItems.FindAsync(request.Id);

            if (existing != null)
            {
                _context.ListItems.Remove(existing);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
