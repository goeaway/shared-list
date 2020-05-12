using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence;

namespace SharedList.API.Application.Queries.GetListsForUser
{
    public class GetListsForUserHandler : IRequestHandler<GetListsForUserRequest, IEnumerable<ListDTO>>
    {
        private readonly SharedListContext _context;
        private readonly IMapper _mapper;

        public GetListsForUserHandler(SharedListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListDTO>> Handle(GetListsForUserRequest request, CancellationToken cancellationToken)
        {
            var contributions = _context.ListContributors.Where(lc => lc.UserIdent == request.UserIdent);
            var lists = _context.Lists.Where(l => contributions.Any(c => c.ListId == l.Id));

            return _mapper.Map<IEnumerable<ListDTO>>(lists);
        }
    }
}
