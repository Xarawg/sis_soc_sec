using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorProcessingOrderInputModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int Action { get; set; }
    }
}
