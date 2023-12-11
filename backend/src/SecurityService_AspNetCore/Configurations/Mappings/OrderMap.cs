using AutoMapper;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.Enums;

namespace SecurityService_AspNetCore.Configurations.Mappings
{
    public class OrderMap : Profile
    {
        public OrderMap()
        {
            CreateMap<OrderDB, OrderDTO>();
        }
    }
}
