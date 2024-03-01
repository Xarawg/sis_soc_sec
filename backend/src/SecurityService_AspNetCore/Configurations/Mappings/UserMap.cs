using AutoMapper;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;

namespace SecurityService_AspNetCore.Configurations.Mappings
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<UserDB, UserDTO>();
            CreateMap<UserDB, UserRegistrationInputModel>();
            CreateMap<UserDB, AdminRegistrationInputModel>();
            CreateMap<UserDB, AdminChangeInputModel>();
        }
    }
}
