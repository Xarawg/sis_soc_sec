namespace SecurityService_Core.Models.DB
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public byte[]? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public int? UserRole { get; set; }
        public string? FIO { get; set; }
        public string? Organization { get; set; }
        public int? INN { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
    }
}