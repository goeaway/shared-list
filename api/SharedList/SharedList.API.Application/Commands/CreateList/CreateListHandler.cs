﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Core.Abstractions;
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
                Name = request.DTO.Name,
                Created = _nowProvider.Now,
                Items = request.DTO.Items?.Select(i => new ListItem
                {
                    Value = i.Value,
                    Notes = i.Notes,
                    Completed = i.Completed,
                    Created = _nowProvider.Now
                }).ToList(),
            };

            await _context.Lists.AddAsync(list, cancellationToken);

            await _context.SaveChangesAsync();

            return list.Id;
        }
    }
}