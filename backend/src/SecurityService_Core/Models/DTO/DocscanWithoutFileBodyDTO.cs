namespace SecurityService_Core.Models.DTO
{
    public class DocscanWithoutFileBodyDTO
    {
        public Guid Id { get; set; }
        public Guid IdOrder { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
    }
}
