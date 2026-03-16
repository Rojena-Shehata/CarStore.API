using Microsoft.AspNetCore.Identity;

namespace CarStore.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}