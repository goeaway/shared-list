using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedList.API.Application.Exceptions;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence;

namespace SharedList.API.Application.Queries.GetList
{
    public class GetListHandler : IRequestHandler<GetListRequest, ListDTO>
    {
        private readonly SharedListContext _context;
        private readonly IMapper _mapper;

        public GetListHandler(SharedListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListDTO> Handle(GetListRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Lists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == request.Id);

            if (list == null)
            {
                throw new RequestFailedException($"Could not find list with id {request.Id}", HttpStatusCode.NotFound);
            }

            list.Items = list.Items.OrderBy(i => i.Order).ToList();
            return _mapper.Map<ListDTO>(list);
        }
    }
}
