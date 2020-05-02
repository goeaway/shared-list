using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Commands.UpdateList
{
    public class UpdateListValidator : AbstractValidator<UpdateListRequest>
    {
        public UpdateListValidator()
        {
            RuleFor(x => x.DTO).NotNull().WithMessage("DTO must not be null");
            When(x => x.DTO != null, () =>
            {
                RuleFor(x => x.DTO.Id).NotEmpty().WithMessage("DTO Id must not be empty");
            });
        }
    }
}
