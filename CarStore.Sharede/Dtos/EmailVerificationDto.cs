
namespace CarStore.Shared.Dtos
{
    public sealed record EmailVerificationDto
        (
            string Email,
            string Token
        );
}