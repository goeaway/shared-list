using System;
using System.Collections.Generic;
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

        public CreateListHandler(SharedListContext context, INowProvider nowProvider)
        {
            _context = context;
            _nowProvider = nowProvider;
        }

        public async Task<string> Handle(CreateListRequest request, CancellationToken cancellationToken)
        {
            var list = new List
            {
                Name = request.Name,
                Created = _nowProvider.Now
            };

            await _context.Lists.AddAsync(list, cancellationToken);

            await _context.SaveChangesAsync();

            return list.Id;
        }
    }
}
