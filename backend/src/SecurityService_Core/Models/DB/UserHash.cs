namespace SecurityService_Core.Models.DB
{
    public class UserHash
    {
        public Guid IdUser { get; set; }
        public string? UserName { get; set; }
        public byte[]? Hash { get; set; }
        public string? Salt { get; set; }
        public int? Status { get; set; }
    }
}
