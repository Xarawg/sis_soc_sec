using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string? State { get; set; }
        public int? Status { get; set; }
        public string? SNILS { get; set; }
        public string? FIO { get; set; }
        public string? ContactData { get; set; }
        public string? Type { get; set; }
        public string? SupportMeasures { get; set; }
    }
}
