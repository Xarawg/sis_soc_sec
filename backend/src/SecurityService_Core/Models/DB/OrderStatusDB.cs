using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityService_Core.Models.DB
{
    public class OrderStatusDB
    {
        public int Id { get; set; }
        public string OrderStatusName { get; set; }
    }
}
