using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Commands.DeleteList
{
    public class DeleteListValidator : AbstractValidator<DeleteListRequest>
    {
        public DeleteListValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id must not be null");
        }
    }
}
