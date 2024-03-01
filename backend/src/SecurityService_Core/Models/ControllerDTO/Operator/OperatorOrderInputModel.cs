using Microsoft.AspNetCore.Http;

namespace SecurityService_Core.Models.ControllerDTO.Operator
{
    public class OperatorOrderInputModel
    {
        public string? SNILS { get; set; }
        public string? FIO { get; set; }
        public string? ContactData { get; set; }
        public string? Type { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}
