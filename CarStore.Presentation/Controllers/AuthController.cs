using CarStore.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using CarStore.ServicesAbstraction;
using CarStore.Shared.CommonResult;

namespace CarStore.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ApiBaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            Result<AuthResponseDto> result = await _authService.LoginAsync(loginUserDto.Email, loginUserDto.Password);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("check-account")]
        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountDto checkAccountDto)
        {
            Result<AccountFoundDto?> result = await _authService.CheckExistingAccountAsync(checkAccountDto.Email, checkAccountDto.Phone);

            return HandleResult<AccountFoundDto?>(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            Result<AuthResponseDto> result = await _authService.RegisterAsync(registerUserDto.FullName, registerUserDto.Email, registerUserDto.Phone, registerUserDto.Password);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> SendEmailVerificationToken([FromBody] ResendEmailVerificationDto resendEmailVerificationDto)
        {
            Result result = await _authService.SendEmailVerificationTokenAsync(resendEmailVerificationDto.Email);

            return HandleResult(result);
        }

        [HttpPost("email-verification")]
        public async Task<IActionResult> EmailVerification([FromBody] EmailVerificationDto emailVerificationDto)
        {
            Result<AuthResponseDto> result = await _authService.EmailVerificationAsync(emailVerificationDto.Email, emailVerificationDto.Code);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            Result result = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
            return HandleResult(result);
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto verifyResetCodeDto)
        {
            Result result = await _authService.VerifyResetCodeAsync(verifyResetCodeDto.Email, verifyResetCodeDto.Code);

            return HandleResult(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            Result result = await _authService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Code, resetPasswordDto.NewPassword, resetPasswordDto.ConfirmNewPassword);

            return HandleResult(result);
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleAuthRequestDto googleAuthRequestDto)
        {
            Result<AuthResponseDto> result = await _authService.GoogleSignInAsync(googleAuthRequestDto.IdToken);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("google-signup")]
        public async Task<IActionResult> GoogleSignUp([FromBody] GoogleAuthRequestDto googleAuthRequestDto)
        {
            Result<AuthResponseDto> result = await _authService.GoogleSignUpAsync(googleAuthRequestDto.IdToken);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            Result<AuthResponseDto> result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);

            return HandleResult<AuthResponseDto>(result);
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenDto revokeRefreshTokenDto)
        {
            Result result = await _authService.RevokeRefreshTokenAsync(revokeRefreshTokenDto.RefreshToken);

            return HandleResult(result);
        }
    }
}