using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
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
            var list = await _context.Lists.FindAsync(request.Id);

            if (list == null)
            {
                throw new RequestFailedException($"Could not find list with id {request.Id}");
            }

            return _mapper.Map<ListDTO>(list);
        }
    }
}
