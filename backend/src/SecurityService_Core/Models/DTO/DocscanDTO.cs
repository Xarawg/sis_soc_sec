namespace SecurityService_Core.Models.DTO
{
    public class DocscanDTO
    {
        public Guid IdDoc { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public byte[] FileBody { get; set; }
    }
}
