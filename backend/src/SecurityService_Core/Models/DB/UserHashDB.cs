namespace SecurityService_Core.Models.DB
{
    public class UserHashDB
    {
        public Guid IdUser { get; set; }
        public string? UserName { get; set; }
        public byte[]? Hash { get; set; }
        public string? Salt { get; set; }
        public int? Status { get; set; }
    }
}
