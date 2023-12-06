using AutoMapper;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.DB;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class AdministratorService
    {
        private readonly IMapper _mapper;
        private readonly IAdministratorStore _administratorStore;

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="administratorStore"></param>
        public AdministratorService(IMapper mapper, IAdministratorStore administratorStore)
        {
            _mapper = mapper;
            _administratorStore = administratorStore;
        }

        public async Task<bool> RegisterAsync(AdminRegistrationDTO model)
        {
            return true;
        }

        public async Task<bool> ChangeUserAsync(AdminRegistrationDTO model)
        {
            return true;
        }

        public async Task<AdminChangePasswordDTO> ChangePasswordAsync(AdminChangePasswordDTO model)
        {
            return new AdminChangePasswordDTO();
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _administratorStore.GetUsersAsync();
            if (users == null)
            {
                throw new Exception("Пользователи не найдены");
            }
            var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users).ToList();
            return result;
        }
    }
}
