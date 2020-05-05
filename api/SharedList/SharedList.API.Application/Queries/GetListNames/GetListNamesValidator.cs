using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SharedList.API.Application.Queries.GetListName;

namespace SharedList.API.Application.Queries.GetListNames
{
    public class GetListNamesValidator : AbstractValidator<GetListNamesRequest>
    {
        public GetListNamesValidator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty().WithMessage("Must have at least one id");
        }
    }
}
