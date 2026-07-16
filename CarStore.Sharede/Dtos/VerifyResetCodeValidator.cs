using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.Dtos
{
        public class VerifyResetCodeValidator : AbstractValidator<VerifyResetCodeDto>
        {
            public VerifyResetCodeValidator()
            {
                RuleFor(x => x.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");

                RuleFor(x => x.Code)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Verification code is required.")
                    .Length(4).WithMessage("Verification code must be 4 digits.")
                    .Matches("^[0-9]{4}$").WithMessage("Verification code must contain digits only.");
            }
        }
    }