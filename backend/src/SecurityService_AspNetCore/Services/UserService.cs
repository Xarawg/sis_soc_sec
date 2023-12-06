using AutoMapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.User;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IUserStore _userStore;

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userStore"></param>
        public UserService(IMapper mapper, IUserStore userStore)
        {
            _mapper = mapper;
            _userStore = userStore;
        }
        
        public async Task<bool> RegisterAsync(UserRegistrationDTO model)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> ChangePasswordAsync(UserChangePasswordDTO model)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> AuthAsync(UserAuthDTO model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <returns>Список </returns>
        /// <exception cref="Exception">Файлы не найдены</exception>
        //public async Task<List<DataDTO>> GetWeatherDataAsync(GetWeatherDataModel model)
        //{
        //    var weatherData = await _userStore.GetWeatherDataAsync(model);
        //    if (weatherData == null)
        //    {
        //        throw new Exception("не найден");
        //    }
        //    var result = _mapper.Map<IEnumerable<WeatherData>, List<WeatherDataDTO>>(weatherData);
        //    return result;
        //}
    }
}
