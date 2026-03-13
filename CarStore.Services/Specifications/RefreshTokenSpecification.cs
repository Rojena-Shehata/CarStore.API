using CarStore.Domain.Entities.IdentityModule;

namespace CarStore.Services.Specifications
{
    public class RefreshTokenSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenSpecification(string refreshToken)
            : base(r => r.Token == refreshToken) { }
    }
}