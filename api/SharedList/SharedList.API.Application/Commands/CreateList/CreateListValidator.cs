using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Commands.CreateList
{
    public class CreateListValidator : AbstractValidator<CreateListRequest>
    {
        public CreateListValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Name must not be null");
        }
    }
}
