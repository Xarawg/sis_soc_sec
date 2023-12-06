namespace SecurityService_Core.Models.DTO
{
    public class UserDTO
    {
        public Guid IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? UserRole { get; set; }
        public string? FIO { get; set; }
        public string? Organization { get; set; }
        public string? INN { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
    }
}