using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Core.Abstractions;
using SharedList.Persistence;

namespace SharedList.API.Application.Commands.UpdateList
{
    public class UpdateListHandler : IRequestHandler<UpdateListRequest>
    {
        private readonly SharedListContext _context;
        private readonly INowProvider _nowProvider;

        public UpdateListHandler(SharedListContext context, INowProvider nowProvider)
        {
            _context = context;
            _nowProvider = nowProvider;
        }

        public async Task<Unit> Handle(UpdateListRequest request, CancellationToken cancellationToken)
        {
            var existing = await _context.Lists.FindAsync(request.DTO.Id);

            if (existing != null)
            {
                existing.Name = request.DTO.Name;
                existing.Updated = _nowProvider.Now;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
