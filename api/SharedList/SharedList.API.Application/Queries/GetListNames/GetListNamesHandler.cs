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

namespace SharedList.API.Application.Queries.GetListName
{
    public class GetListNamesHandler : IRequestHandler<GetListNamesRequest, IEnumerable<ListNameAndIdDTO>>
    {
        private readonly SharedListContext _context;
        private readonly IMapper _mapper;

        public GetListNamesHandler(SharedListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListNameAndIdDTO>> Handle(GetListNamesRequest request, CancellationToken cancellationToken)
        {
            return _context
                .Lists
                .Where(l => request.Ids.Contains(l.Id))
                .Select(l => _mapper.Map<ListNameAndIdDTO>(l));
        }
    }
}
