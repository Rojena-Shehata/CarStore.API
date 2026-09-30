using CarStore.Shared.Dtos;
using FluentValidation;

namespace CarStore.Services.Validation.AuthValidation
{
    public class EmailVerificationValidator : AbstractValidator<EmailVerificationDto>
    {
        public EmailVerificationValidator()
        {
            RuleFor(x => x.Email)
              .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage("Email is required.")
              .EmailAddress().WithMessage("A valid email required.")
              .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");

            RuleFor(x => x.Code)
               .NotEmpty().WithMessage("Token is required.");
        }
    }
}