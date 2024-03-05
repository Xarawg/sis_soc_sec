namespace SecurityService_Core.Models.DTO
{
    public class UserDTO
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserRole { get; set; }
        public string? FIO { get; set; }
        public string? Organization { get; set; }
        public string? INN { get; set; }
        public string? Address { get; set; }
        public int State { get; set; }
        public string? Status { get; set; }
    }
}