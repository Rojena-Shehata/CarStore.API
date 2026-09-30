using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.Dtos
{
        public class CheckAccountValidator : AbstractValidator<CheckAccountDto>
        {
            public CheckAccountValidator()
            {
                RuleFor(x => x.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");

                RuleFor(x => x.Phone)
                    .NotEmpty().WithMessage("Phone number is required.");
            }
        }
    }
