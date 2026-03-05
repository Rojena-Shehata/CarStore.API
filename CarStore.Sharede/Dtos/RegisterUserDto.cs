
namespace CarStore.Shared.Dtos
{
    public sealed record RegisterUserDto
        (
            string FullName,
            string Email,
            string Phone,
            string Password
        );
}