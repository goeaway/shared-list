﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SharedList.API.Application.Queries.GetList
{
    public class GetListValidator : AbstractValidator<GetListRequest>
    {
        public GetListValidator()
        {
            RuleFor(model => model.Id)
                .NotEmpty()
                .WithMessage("Id not set");

            RuleFor(model => model.UserIdent).NotEmpty().WithMessage("UserId not set");
        }
    }
}
