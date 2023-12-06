using AutoMapper;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;

namespace SecurityService_AspNetCore.Configurations.Mappings
{
    public class DocscanMap : Profile
    {
        public DocscanMap()
        {
            CreateMap<Docscan, DocscanDTO>();
        }
    }
}
