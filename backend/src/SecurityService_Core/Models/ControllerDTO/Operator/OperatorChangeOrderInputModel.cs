using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorChangeOrderInputModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? SNILS { get; set; }
        [Required]
        public string? FIO { get; set; }
        [Required]
        public string? ContactData { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public string? SupportMeasures { get; set; }
    }
}
