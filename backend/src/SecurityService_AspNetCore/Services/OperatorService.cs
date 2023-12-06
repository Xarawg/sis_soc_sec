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
using SecurityService_Core.Models.DB;
using SecurityService_Core_Stores.Stores;

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
            var orders = await _operatorStore.GetOrdersAsync();
            if (orders == null)
            {
                throw new Exception("Заявки не найдены");
            }
            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders).ToList();
            return result;
        }

        public async Task<bool> CreateOrderAsync(OperatorOrderDTO model)
        {
            return true;
        }

        public async Task<bool> ChangeOrderAsync(OperatorOrderDTO model)
        {
            return true;
        }
    }
}
