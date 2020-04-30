using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Persistence;

namespace SharedList.API.Application.Commands.DeleteList
{
    public class DeleteListHandler : IRequestHandler<DeleteListRequest>
    {
        private readonly SharedListContext _context;

        public DeleteListHandler(SharedListContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteListRequest request, CancellationToken cancellationToken)
        {
            var existing = await _context.Lists.FindAsync(request.Id);

            if (existing != null)
            {
                _context.Lists.Remove(existing);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
