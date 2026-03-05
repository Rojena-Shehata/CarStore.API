
namespace CarStore.Shared.Dtos
{
    public sealed record ResetPasswordDto
        (
            string Email,
            string Token,
            string NewPassword,
            string ConfirmNewPassword
        );
}