using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedList.API.Application.Exceptions;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.Entities;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Application.Commands.UpdateList
{
    public class UpdateListHandler : IRequestHandler<UpdateListRequest>
    {
        private readonly SharedListContext _context;
        private readonly INowProvider _nowProvider;
        private readonly IConfiguration _configuration;

        public UpdateListHandler(SharedListContext context, INowProvider nowProvider, IConfiguration configuration)
        {
            _context = context;
            _nowProvider = nowProvider;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(UpdateListRequest request, CancellationToken cancellationToken)
        {
            var itemLimit = _configuration.GetValue<int>("Limits:ListItems");
            if (request.DTO.Items.Count >= itemLimit)
            {
                throw new RequestFailedException($"List Item limit reached ({itemLimit})", System.Net.HttpStatusCode.Forbidden);
            }

            var existing = await _context.Lists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == request.DTO.Id);

            if (existing != null)
            {
                existing.Name = request.DTO.Name?.Trim();
                existing.Updated = _nowProvider.Now;
                // remove the old items as we're going to overwrite them
                foreach (var removedItem in existing.Items)
                {
                    _context.ListItems.Remove(removedItem);
                }

                existing.Items = request.DTO.Items?.Select((i, index) => new ListItem
                {
                    Id = i.Id.Trim(),
                    Order = index,
                    Value = i.Value?.Trim(),
                    Created = _nowProvider.Now,
                    Notes = i.Notes?.Trim(),
                    Completed = i.Completed,
                    ParentList = existing
                }).ToList();

                // find all existing contributions
                // if there is none for this list and user add a new one
                if (!await _context.ListContributors.AnyAsync(lc =>
                    lc.ListId == existing.Id && lc.UserIdent == request.UserIdent))
                {
                    var newContributor = new ListContributor
                    {
                        ListId = existing.Id,
                        UserIdent = request.UserIdent
                    };

                    await _context.ListContributors.AddAsync(newContributor);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
