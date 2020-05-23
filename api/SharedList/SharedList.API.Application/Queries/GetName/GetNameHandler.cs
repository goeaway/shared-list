using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharedList.Core.Abstractions;

namespace SharedList.API.Application.Queries.GetName
{
    public class GetNameHandler : IRequestHandler<GetNameRequest, string>
    {
        private readonly IRandomisedWordProvider _randomisedWordProvider;

        public GetNameHandler(IRandomisedWordProvider randomisedWordProvider)
        {
            _randomisedWordProvider = randomisedWordProvider;
        }

        public async Task<string> Handle(GetNameRequest request, CancellationToken cancellationToken)
        {
            return _randomisedWordProvider.CreateRandomName();
        }
    }
}
