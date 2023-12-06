using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorOrderDTO
    {
        public DateTime? Date { get; set; }
        public string? State { get; set; }
        public string? SNILS { get; set; }
        public string? FIO { get; set; }
        public string? ContactData { get; set; }
        public string? Type { get; set; }
        public string? SupportMeasures { get; set; }
    }
}
