using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Commands.UpdateListItem
{
    public class UpdateListItemValidator : AbstractValidator<UpdateListItemRequest>
    {
        public UpdateListItemValidator()
        {
            RuleFor(x => x.DTO).NotNull().WithMessage("DTO must not be null");

            When(x => x.DTO != null,
                () =>
                {
                    RuleFor(x => x.DTO.Id).GreaterThan(0).WithMessage("DTO Id must be greater than 0"); 

                });
        }
    }
}
