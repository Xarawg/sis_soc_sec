using AutoMapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.ControllerDTO.Operator;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class OperatorService
    {
        private readonly IMapper _mapper;
        private readonly IOperatorStore _operatorStore;

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="operatorStore"></param>
        public OperatorService(IMapper mapper, IOperatorStore operatorStore)
        {
            _mapper = mapper;
            _operatorStore = operatorStore;
        }
        
        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            return new List<OrderDTO>();
        }

        public async Task<bool> CreateOrderAsync(OperatorOrderDTO model)
        {
            return true;
        }

        public async Task<bool> ChangeOrderAsync(OperatorOrderDTO model)
        {
            return true;
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <returns>Список </returns>
        /// <exception cref="Exception">Файлы не найдены</exception>
        //public async Task<List<DataDTO>> GetWeatherDataAsync(GetWeatherDataModel model)
        //{
        //    var weatherData = await _operatorStore.GetWeatherDataAsync(model);
        //    if (weatherData == null)
        //    {
        //        throw new Exception("не найден");
        //    }
        //    var result = _mapper.Map<IEnumerable<WeatherData>, List<WeatherDataDTO>>(weatherData);
        //    return result;
        //}
    }
}
