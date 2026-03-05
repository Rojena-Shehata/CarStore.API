using FluentValidation;
using CarStore.Shared.Dtos;

namespace CarStore.Services.Validation.AuthValidation
{
    public class RefreshTokenValidation : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidation()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}