namespace SecurityService_Core.Models.DB
{
    public class DocscanDB : BaseEntity
    {
        public Guid IdOrder { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public byte[] FileBody { get; set; }
    }
}
