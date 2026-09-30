
namespace CarStore.Shared.Dtos
{
    public sealed record AuthResponseDto
        (
            string FullName,
            string Email,
            string AccessToken,
            string RefreshToken,
            DateTime AccessTokenExpiresAt
        );
}