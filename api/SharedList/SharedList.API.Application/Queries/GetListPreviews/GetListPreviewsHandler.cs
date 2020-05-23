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
    public class GetListPreviewsHandler : IRequestHandler<GetListPreviewsRequest, IEnumerable<ListPreviewDTO>>
    {
        private readonly SharedListContext _context;
        private readonly IMapper _mapper;

        public GetListPreviewsHandler(SharedListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListPreviewDTO>> Handle(GetListPreviewsRequest request, CancellationToken cancellationToken)
        {
            // get contributions for this user
            var contributions = _context.ListContributors.Where(lc => lc.UserIdent == request.UserIdent);
            // get all lists with those contrinbutions
            var lists = _context
                .Lists
                .Where(l => contributions.Any(c => c.ListId == l.Id))
                .OrderByDescending(l => l.Updated);

            // map to preview
            var mapped = _mapper.Map<IEnumerable<ListPreviewDTO>>(lists);

            // find other contributors for each
            foreach (var list in mapped)
            {
                list.OtherContributors =
                    _context.ListContributors
                        .Where(lc => lc.UserIdent != request.UserIdent && lc.ListId == list.Id)
                        .Select(lc => lc.UserIdent);
            }

            return mapped;
        }
    }
}
