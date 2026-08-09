namespace CarStore.Domain.Entities.IdentityModule
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }

        // Navigation Properties
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}