using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.Entities;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Application.Commands.CreateList
{
    public class CreateListHandler : IRequestHandler<CreateListRequest, string>
    {
        private readonly SharedListContext _context;
        private readonly INowProvider _nowProvider;
        private readonly IRandomisedWordProvider _randomisedWordProvider;

        public CreateListHandler(SharedListContext context, INowProvider nowProvider, IRandomisedWordProvider randomisedWordProvider)
        {
            _context = context;
            _nowProvider = nowProvider;
            _randomisedWordProvider = randomisedWordProvider;
        }

        public async Task<string> Handle(CreateListRequest request, CancellationToken cancellationToken)
        {
            var list = new List
            {
                Id = _randomisedWordProvider.CreateWordsString(),
                Name = request.DTO.Name?.Trim(),
                Created = _nowProvider.Now,
                Updated = _nowProvider.Now,
                Items = request.DTO.Items?.Select((i, index) => new ListItem
                {
                    Id = i.Id.Trim(),
                    Order = index,
                    Value = i.Value?.Trim(),
                    Notes = i.Notes?.Trim(),
                    Completed = i.Completed,
                    Created = _nowProvider.Now
                }).ToList()
            };

            var contribution = new ListContributor
            {
                ListId = list.Id,
                UserIdent = request.UserIdent
            };

            await _context.Lists.AddAsync(list, cancellationToken);
            await _context.ListContributors.AddAsync(contribution, cancellationToken);

            await _context.SaveChangesAsync();

            return list.Id;
        }
    }
}
