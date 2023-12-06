using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DTO;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
    [Route("api/operator")]
    public class OperatorController : BaseController
    {
        private readonly ILogger<OperatorController> _logger;
        private readonly OperatorService _operatorService;

        /// <summary>
        /// Конструктор контроллера файлов
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="logService"></param>
        /// <param name="operatorService"></param>
        public OperatorController(ILogger<OperatorController> logger, OperatorService operatorService)
        {
            _logger = logger;
            _operatorService = operatorService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Список заявок</returns>
        [HttpGet]
        [Route("get-orders")]
        public async Task<IResult> GetOrders()
        {
            try
            {
                var result = await _operatorService.GetOrdersAsync();

                return Results.Json(Result<List<OrderDTO>>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-order")]
        public async Task<IResult> CreateOrder([FromBody] OperatorOrderDTO model)
        {
            try
            {
                var result = await _operatorService.CreateOrderAsync(model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-order")]
        public async Task<IResult> ChangeOrder([FromBody] OperatorOrderDTO model)
        {
            try
            {
                var result = await _operatorService.ChangeOrderAsync(model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
