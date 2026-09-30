using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using CarStore.Domain.Entities.IdentityModule;
using CarStore.Domain.Enums;
using CarStore.Services.Specifications;
using CarStore.ServicesAbstraction;
using CarStore.Shared.CommonResult;
using CarStore.Shared.Dtos;
using CarStore.Shared.Settings;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarStore.Services.Services
{
    public class AuthService(
         UserManager<ApplicationUser> _userManager,
         IEmailService _emailService,
         ITokenService _tokenService,
         IOptions<JwtSettings> _jwtOptions,
         IOptions<GoogleAuthSettings> _googleOptions,
         IHttpContextAccessor _httpContextAccessor,
         IIdentityUnitOfWork _identityUnitOfWork) : IAuthService
    {


        public async Task<Result<AuthResponseDto>> LoginAsync(string email, string password)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Error.Validation(description: "Email is not confirmed. Please verify your email before logging in.");

            if (!await _userManager.CheckPasswordAsync(user, password))
                return Error.InvalidCredentials(description: "Email or Password is incorrect.");

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(string fullName, string email, string phone, string password)
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
                return Error.Validation(code: "Error.EmailInUse", description: "User with this email already exist.");

            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == phone))
                return Error.Validation(code: "Error.PhoneInUse", description: "An account with this phone number already exists.");

            ApplicationUser user = new()
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                UserName = email.Split('@')[0],
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);
            if (!identityResult.Succeeded)
                return Error.Validation(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            // Handle role later

            await SendEmailVerificationTokenAsync(email);

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result<AccountFoundDto?>> CheckExistingAccountAsync(string email, string phone)
        {
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(email)
                ?? await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);

            if (existingUser is null)
                return Result<AccountFoundDto?>.Ok(null);

            return Result<AccountFoundDto?>.Ok(new AccountFoundDto(existingUser.FullName, existingUser.Email!, existingUser.PhoneNumber ?? string.Empty));
        }

        public async Task<Result> SendEmailVerificationTokenAsync(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return Error.Validation(description: "Email is already confirmed.");

            string code = await generateAndStoreCodeAsync(email, VerificationCodePurpose.EmailConfirmation);

            string htmlBody = emailVerificationHtmlBody(code);
            await _emailService.SendEmailAsync(email, "Motor AI - Email Verification", htmlBody);

            return Result.Ok();
        }

        public async Task<Result<AuthResponseDto>> EmailVerificationAsync(string email, string code)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return Error.Validation(description: "Email is already confirmed.");

            var codeResult = await validateCodeAsync(email, code, VerificationCodePurpose.EmailConfirmation);
            if (codeResult.IsFailed)
                return codeResult.Errors.ToList();

            VerificationCode verificationCode = codeResult.Value;
            verificationCode.IsUsed = true;
            _identityUnitOfWork.GetRepository<VerificationCode, int>().Update(verificationCode);

            user.EmailConfirmed = true;
            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
                return Error.Failure(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            await _identityUnitOfWork.SaveChangesAsync();

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result> ForgotPasswordAsync(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            string code = await generateAndStoreCodeAsync(email, VerificationCodePurpose.PasswordReset);

            string htmlBody = forgetPasswordHtmlBody(code);
            await _emailService.SendEmailAsync(email, "Motor AI - Reset Password", htmlBody);

            return Result.Ok();
        }

        public async Task<Result> VerifyResetCodeAsync(string email, string code)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            var codeResult = await validateCodeAsync(email, code, VerificationCodePurpose.PasswordReset);
            if (codeResult.IsFailed)
                return codeResult.Errors.ToList();

            VerificationCode verificationCode = codeResult.Value;
            verificationCode.IsVerified = true;
            _identityUnitOfWork.GetRepository<VerificationCode, int>().Update(verificationCode);
            await _identityUnitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> ResetPasswordAsync(string email, string code, string newPassword, string confirmNewPassword)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound(description: "User with this email not found.");

            var codeResult = await validateCodeAsync(email, code, VerificationCodePurpose.PasswordReset);
            if (codeResult.IsFailed)
                return codeResult.Errors.ToList();

            VerificationCode verificationCode = codeResult.Value;
            if (!verificationCode.IsVerified)
                return Error.Validation(description: "Please verify the code before resetting the password.");

            string identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, identityToken, newPassword);
            if (!identityResult.Succeeded)
                return Error.Failure(description: string.Join(", ", identityResult.Errors.Select(e => e.Description).ToList()));

            verificationCode.IsUsed = true;
            _identityUnitOfWork.GetRepository<VerificationCode, int>().Update(verificationCode);
            await _identityUnitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result<AuthResponseDto>> GoogleSignInAsync(string idToken)
        {
            var payloadResult = await ValidateGoogleTokenAsync(idToken);
            if (payloadResult.IsFailed)
                return payloadResult.Errors.ToList();

            var payload = payloadResult.Value;

            ApplicationUser? user = await _userManager.FindByLoginAsync(GoogleDefaults.AuthenticationScheme, payload.Subject);
            if (user == null)
                return Error.NotFound(description: "user not found. Please sign up first.");

            var logins = await _userManager.GetLoginsAsync(user);
            if (!logins.Any(l => l.LoginProvider == GoogleDefaults.AuthenticationScheme))
                return Error.Validation(description: "This account is not registered using Google.");

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponseDto>> GoogleSignUpAsync(string idToken)
        {
            var payloadResult = await ValidateGoogleTokenAsync(idToken);
            if (payloadResult.IsFailed)
                return payloadResult.Errors.ToList();

            var payload = payloadResult.Value;

            var uxistingUser = await _userManager.FindByEmailAsync(payload.Email);
            if (uxistingUser != null)
                return Error.Validation(description: "user with this email already exists, Please sign in instead.");

            ApplicationUser newUser = new()
            {
                UserName = payload.Email.Split('@')[0],
                Email = payload.Email,
                EmailConfirmed = true,
                FullName = payload.Name
            };

            IdentityResult result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
                return Error.Failure(description: "Failed to create user account.");

            var loginResult = await _userManager.AddLoginAsync(
                newUser,
                new UserLoginInfo(GoogleDefaults.AuthenticationScheme, payload.Subject, GoogleDefaults.AuthenticationScheme));

            if (!loginResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                return Error.Failure(description: "Failed to link Google account.");
            }

            return await generateAuthResponseAsync(newUser);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            RefreshTokenSpecification refreshTokenSpecification = new RefreshTokenSpecification(refreshToken);
            RefreshToken? existingRefreshToken = await _identityUnitOfWork.GetRepository<RefreshToken, int>()
                .GetByIdAsync(refreshTokenSpecification);

            if (existingRefreshToken == null)
                return Error.Validation(description: "Invalid Refresh Token.");
            if (existingRefreshToken.RevokedOn != null)
                return Error.Validation(description: "Refresh Token has been revoked.");
            if (existingRefreshToken.ExpiresOn <= DateTime.UtcNow)
                return Error.Validation(description: "Refresh Token has expired.");

            ApplicationUser? user = await _userManager.FindByIdAsync(existingRefreshToken.UserId);
            if (user == null)
                return Error.NotFound(description: "User not found.");

            existingRefreshToken.RevokedOn = DateTime.UtcNow;
            _identityUnitOfWork.GetRepository<RefreshToken, int>().Update(existingRefreshToken);
            await _identityUnitOfWork.SaveChangesAsync();

            return await generateAuthResponseAsync(user);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string refreshToken)
        {
            RefreshTokenSpecification refreshTokenSpecification = new RefreshTokenSpecification(refreshToken);
            RefreshToken? existingRefreshToken = await _identityUnitOfWork.GetRepository<RefreshToken, int>()
                .GetByIdAsync(refreshTokenSpecification);

            if (existingRefreshToken == null)
                return Error.Validation(description: "Invalid Refresh Token.");
            if (existingRefreshToken.RevokedOn != null)
                return Error.Validation(description: "Refresh Token has been revoked.");
            if (existingRefreshToken.ExpiresOn <= DateTime.UtcNow)
                return Error.Validation(description: "Refresh Token has expired.");

            if (existingRefreshToken.UserId != (_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Error.Unauthorized();

            existingRefreshToken.RevokedOn = DateTime.UtcNow;
            _identityUnitOfWork.GetRepository<RefreshToken, int>().Update(existingRefreshToken);
            await _identityUnitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        // Helper Methods
        private const int CodeExpirationMinutes = 10;
        private const int MaxAttempts = 5;

        private async Task<string> generateAndStoreCodeAsync(string email, VerificationCodePurpose purpose)
        {
            // Invalidate any previous unused codes for the same email/purpose
            var repository = _identityUnitOfWork.GetRepository<VerificationCode, int>();
            var previousCodes = await repository.GetAllAsync(new ActiveVerificationCodeSpecification(email, purpose));
            foreach (var previousCode in previousCodes)
            {
                previousCode.IsUsed = true;
                repository.Update(previousCode);
            }

            string code = RandomNumberGenerator.GetInt32(0, 10000).ToString("D4");

            VerificationCode verificationCode = new()
            {
                Email = email,
                Purpose = purpose,
                CodeHash = hashCode(code),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddMinutes(CodeExpirationMinutes)
            };
            repository.Add(verificationCode);

            await _identityUnitOfWork.SaveChangesAsync();

            return code;
        }

        private async Task<Result<VerificationCode>> validateCodeAsync(string email, string code, VerificationCodePurpose purpose)
        {
            var repository = _identityUnitOfWork.GetRepository<VerificationCode, int>();
            VerificationCode? verificationCode = await repository.GetByIdAsync(new ActiveVerificationCodeSpecification(email, purpose));

            if (verificationCode is null)
                return Error.Validation(description: "Invalid or expired verification code.");

            if (verificationCode.ExpiresOn <= DateTime.UtcNow)
                return Error.Validation(description: "Verification code has expired. Please request a new one.");

            if (verificationCode.AttemptsCount >= MaxAttempts)
                return Error.Validation(description: "Too many attempts. Please request a new code.");

            if (verificationCode.CodeHash != hashCode(code))
            {
                verificationCode.AttemptsCount++;
                repository.Update(verificationCode);
                await _identityUnitOfWork.SaveChangesAsync();
                return Error.Validation(description: "Invalid verification code.");
            }

            return verificationCode;
        }

        private static string hashCode(string code)
        {
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(code));
            return Convert.ToHexString(hashBytes);
        }

        private async Task<Result<GoogleJsonWebSignature.Payload>> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var payloadResult = await GoogleJsonWebSignature.ValidateAsync(
                    idToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _googleOptions.Value.ClientId }
                    });

                return payloadResult;
            }
            catch
            {
                return Error.Unauthorized(description: "Invalid Google refreshToken provided.");
            }
        }

        private async Task<AuthResponseDto> generateAuthResponseAsync(ApplicationUser user) =>
             new AuthResponseDto
            (
                user.FullName,
                user.Email!,
                await _tokenService.CreateAccessTokenAsync(user),
                await _tokenService.CreateRefreshTokenAsync(user.Id),
                DateTime.UtcNow.AddMinutes(_jwtOptions.Value.AccessTokenExpirationMinutes)
            );

        private string emailVerificationHtmlBody(string token) =>
           $@"
            <div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQnJpS4xY3itSCDGZF_c0ekzP-vnEnNWvoF3CEI2gJjHxe3doc3' alt='Motor AI' style='width:120px;'>
                </div>
                <h2 style='color:#2c3e50; text-align:center;'>Verify Your Email</h2>
                <p style='color:#555;'>Hello,</p>
                <p style='color:#555;'>We received a request to verify your email. Use the code below to continue:</p>
                <div style='text-align:center; margin:30px 0; word-break:break-all;'>
                   {token}
                </div>
                <p style='color:#555;'>This code will expire soon.</p>
                <p style='color:#888; font-size:12px;'>If you didn’t request this, please ignore this email.</p>
                <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
                <p style='color:#888; font-size:12px; text-align:center;'>Thanks, <br/>The Motor AI Team.</p>
            </div>
            ";

        private string forgetPasswordHtmlBody(string token) =>
           $@"
            <div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQnJpS4xY3itSCDGZF_c0ekzP-vnEnNWvoF3CEI2gJjHxe3doc3' alt='Motor AI' style='width:120px;'>
                </div>
                <h2 style='color:#2c3e50; text-align:center;'>Reset Your Password</h2>
                <p style='color:#555;'>Hello,</p>
                <p style='color:#555;'>We received a request to reset your password. Copy the code below:</p>
                <div style='text-align:center; margin:30px 0;'>
                   {token}
                </div>
                <p style='color:#555;'>This code can only be used once and expires in 10 minutes.</p>
                <p style='color:#888; font-size:12px;'>If you didn’t request this, please ignore this email.</p>
                <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
                <p style='color:#888; font-size:12px; text-align:center;'>Thanks, <br/>The Motor AI Team.</p>
            </div>
            ";
    }
}