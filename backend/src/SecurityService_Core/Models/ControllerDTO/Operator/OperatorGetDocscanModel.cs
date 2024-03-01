using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorGetDocscanModel
    {
        [Required]
        public Guid IdDoc { get; set; }
    }
}
