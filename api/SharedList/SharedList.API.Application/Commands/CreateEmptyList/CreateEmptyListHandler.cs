using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedList.API.Application.Exceptions;
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
        private readonly IConfiguration _configuration;

        public CreateEmptyListHandler(SharedListContext context, INowProvider nowProvider,
            IRandomisedWordProvider randomisedWordProvider, IConfiguration configuration)
        {
            _context = context;
            _nowProvider = nowProvider;
            _randomisedWordProvider = randomisedWordProvider;
            _configuration = configuration;
        }

        public async Task<string> Handle(CreateEmptyListRequest request, CancellationToken cancellationToken)
        {
            // check for existing contributions
            // if this is equal to limit already, throw 403
            var contributionsForUser = _context.ListContributors.Where(lc => lc.UserIdent == request.UserIdent);
            var listsLimit = _configuration.GetValue<int>("Limits:Lists");
            if (contributionsForUser.Count() >= listsLimit)
            {
                throw new RequestFailedException($"List limit reached ({listsLimit})", System.Net.HttpStatusCode.Forbidden);
            }

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
