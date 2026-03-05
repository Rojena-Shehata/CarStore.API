using FluentValidation;
using CarStore.Shared.Dtos;

namespace CarStore.Services.Validation.AuthValidation
{
    public class RevokeRefreshTokenValidator:AbstractValidator<RevokeRefreshTokenDto>
    {
        public RevokeRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}