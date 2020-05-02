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
            RuleFor(x => x.DTO)
                .NotNull()
                .WithMessage("DTO must not be null");

            When(x => x.DTO != null, () =>
            {
                RuleFor(x => x.DTO.Name)
                    .NotEmpty()
                    .WithMessage("Name must not be empty");
            });

        }
    }
}
