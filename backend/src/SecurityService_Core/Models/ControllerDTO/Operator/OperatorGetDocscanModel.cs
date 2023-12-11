using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorGetDocscanModel
    {
        [Required]
        public Guid IdDoc { get; set; }
    }
}
