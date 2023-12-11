using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.DB
{
    public class UserRoleDB
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; }
    }
}
