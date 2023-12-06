using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core.Models.DB
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? CreateUser { get; set; }
        public string? ChangeUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
