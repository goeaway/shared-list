using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Commands.DeleteListItem
{
    public class DeleteListItemValidator : AbstractValidator<DeleteListItemRequest>
    {
        public DeleteListItemValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
