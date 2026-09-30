using Microsoft.AspNetCore.Identity;

namespace CarStore.Domain.Entities.IdentityModule
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}