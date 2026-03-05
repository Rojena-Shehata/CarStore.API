using CarStore.Shared.Dtos;
using CarStore.Shared.CommonResult;

namespace CarStore.ServicesAbstraction
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> LoginAsync(string email, string password);
        Task<Result<AuthResponseDto>> RegisterAsync(string fullName, string email, string phone, string password);
        //Task<Result> LogoutAsync(string refreshToken);

        //Task<Result> SendEmailVerificationTokenAsync(string email);
        //Task<Result<AuthResponseDto>> EmailVerificationAsync(string email, string refreshToken);

        Task<Result> ForgotPasswordAsync(string email);
        Task<Result> ResetPasswordAsync(string email, string token, string newPassword, string confirmNewPassword);

        Task<Result<AuthResponseDto>> GoogleSignInAsync(string idToken);
        Task<Result<AuthResponseDto>> GoogleSignUpAsync(string idToken);

        Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result> RevokeRefreshTokenAsync(string refreshToken);
    }
}