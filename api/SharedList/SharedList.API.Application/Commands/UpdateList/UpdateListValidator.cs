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
            RuleFor(x => x.DTO.Id).NotNull().WithMessage("DTO Id must not be null");
        }
    }
}
