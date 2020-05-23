using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.DTOs;
using SharedList.Core.Models.Entities;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Application.Commands.CreateEmptyList
{
    public class CreateEmptyListHandler : IRequestHandler<CreateEmptyListRequest, string>
    {
        private readonly SharedListContext _context;
        private readonly INowProvider _nowProvider;
        private readonly IRandomisedWordProvider _randomisedWordProvider;

        public CreateEmptyListHandler(SharedListContext context, INowProvider nowProvider,
            IRandomisedWordProvider randomisedWordProvider)
        {
            _context = context;
            _nowProvider = nowProvider;
            _randomisedWordProvider = randomisedWordProvider;
        }

        public async Task<string> Handle(CreateEmptyListRequest request, CancellationToken cancellationToken)
        {
            var list = new List
            {
                Id = _randomisedWordProvider.CreateRandomId(),
                Name = _randomisedWordProvider.CreateRandomName(),
                Created = _nowProvider.Now,
                Updated = _nowProvider.Now,
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
