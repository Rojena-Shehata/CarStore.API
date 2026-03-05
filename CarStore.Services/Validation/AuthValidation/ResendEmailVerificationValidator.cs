using FluentValidation;
using CarStore.Shared.Dtos;

namespace CarStore.Services.Validation.AuthValidation
{
    public class ResendEmailVerificationValidator:AbstractValidator<ResendEmailVerificationDto>
    {
        public ResendEmailVerificationValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");
        }
    }
}