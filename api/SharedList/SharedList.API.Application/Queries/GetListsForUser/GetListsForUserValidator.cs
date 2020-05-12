using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Queries.GetListsForUser
{
    public class GetListsForUserValidator : AbstractValidator<GetListsForUserRequest>
    {
        public GetListsForUserValidator()
        {
            RuleFor(x => x.UserIdent).NotEmpty().WithMessage("UserIdent must not be empty");
        }
    }
}
