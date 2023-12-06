using AutoMapper;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;

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
            return new List<UserDTO>();
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <returns>Список </returns>
        /// <exception cref="Exception">Файлы не найдены</exception>
        //public async Task<List<DataDTO>> GetWeatherDataAsync(GetWeatherDataModel model)
        //{
        //    var weatherData = await _administratorStore.GetWeatherDataAsync(model);
        //    if (weatherData == null)
        //    {
        //        throw new Exception("не найден");
        //    }
        //    var result = _mapper.Map<IEnumerable<WeatherData>, List<WeatherDataDTO>>(weatherData);
        //    return result;
        //}
    }
}
