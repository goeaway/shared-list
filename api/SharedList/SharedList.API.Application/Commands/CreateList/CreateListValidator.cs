using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;

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

                When(x => x.DTO.Items != null && x.DTO.Items.Any(), () =>
                {
                    RuleForEach(x => x.DTO.Items).ChildRules(item =>
                    {
                        item.RuleFor(x => x.Id).NotEmpty().WithMessage("List Item Id must not be empty");
                    });
                });
            });

        }
    }
}
