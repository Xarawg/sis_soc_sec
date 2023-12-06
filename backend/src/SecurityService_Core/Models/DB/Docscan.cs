namespace SecurityService_Core.Models.DB
{
    public class Docscan : BaseEntity
    {
        public Guid IdOrder { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public byte[] FileBody{ get; set; }
    }
}
