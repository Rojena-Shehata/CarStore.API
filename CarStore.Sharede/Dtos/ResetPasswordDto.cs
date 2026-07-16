
namespace CarStore.Shared.Dtos
{
    public sealed record ResetPasswordDto
        (
            string Email,
            string Code,
            string NewPassword,
            string ConfirmNewPassword
        );
}