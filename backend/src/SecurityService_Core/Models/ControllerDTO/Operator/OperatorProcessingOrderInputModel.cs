using System.ComponentModel.DataAnnotations;

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
