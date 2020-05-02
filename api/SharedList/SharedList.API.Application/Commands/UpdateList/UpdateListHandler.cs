using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Core.Abstractions;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

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
                // remove the old items as we're going to overwrite them
                foreach (var removedItem in existing.Items)
                {
                    _context.ListItems.Remove(removedItem);
                }

                existing.Items = request.DTO.Items?.Select(i => new ListItem
                {
                    Value = i.Value,
                    Created = _nowProvider.Now,
                    Notes = i.Notes,
                    Completed = i.Completed,
                    ParentList = existing
                }).ToList();

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
