using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.DB
{
    public class Order : BaseEntity
    {
        public DateTime? Date { get; set; }
        public string? State { get; set; }
        public string? SNILS { get; set; }
        public string? FIO { get; set; }
        public string? ContactData { get; set; }
        public string? Type { get; set; }
        public string? Body { get; set; }
        public string? SupportMeasures { get; set; }
    }
}
