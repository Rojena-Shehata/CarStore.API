using CarStore.Domain.Entities.IdentityModule;

namespace CarStore.ServicesAbstraction
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(ApplicationUser user);
        Task<string> CreateRefreshTokenAsync(string userID);
        //Task<Result> RevokeRefreshTokenAsync(RefreshToken refreshToken);
    }
}