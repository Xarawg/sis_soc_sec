using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.Enums;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/operator")]
    public class OperatorController : BaseController
    {
        private readonly ILogger<OperatorController> _logger;
        private readonly OperatorService _operatorService;
        private readonly UserService _userService;

        public readonly List<int> ACTIONS = Enum.GetValues(typeof(OrderProcessingAction)).Cast<int>().ToList();

        /// <summary>
        /// Конструктор контроллера файлов
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="operatorService"></param>
        public OperatorController(
            ILogger<OperatorController> logger
            , OperatorService operatorService
            ,UserService userService)
        {
            _logger = logger;
            _operatorService = operatorService;
            _userService = userService;
        }

        /// <summary>
        /// Получить список заявок для оператора
        /// </summary>
        /// <returns>Список заявок</returns>
        [HttpPost]
        [Route("get-orders")]
        public async Task<IResult> GetOrders([FromBody] OperatorOrderGetModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Access denied."); // Учётная запись оператора не одобрена администратором

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Access denied");

                var result = await _operatorService.GetOrdersAsync(model);

                return Results.Json(Result<List<OrderDTO>>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Получить список заявок для оператора
        /// </summary>
        /// <returns>Список заявок</returns>
        [HttpPost]
        [Route("get-order-document-by-id")]
        public async Task<IResult> GetOrderDocumentsById([FromBody] OperatorGetDocscanModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Access denied."); // Учётная запись оператора не одобрена администратором

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Access denied");

                var result = await _operatorService.GetDocscanAsync(model.IdDoc);

                return Results.Json(result);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Создать заявку оператором
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Успешность действия</returns>
        [HttpPost]
        [Route("create-order")]
        public async Task<IResult> CreateOrder([FromForm] OperatorOrderInputModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Access denied."); // Учётная запись оператора не одобрена администратором

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Access denied");

                var result = await _operatorService.CreateOrderAsync(model, GetUserName());

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Изменить заявку оператором
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Успешность действия</returns>
        [HttpPost]
        [Route("change-order")]
        public async Task<IResult> ChangeOrder([FromBody] OperatorChangeOrderInputModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Access denied."); // Учётная запись оператора не одобрена администратором

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Access denied");

                var result = await _operatorService.ChangeOrderAsync(model, GetUserName());

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
        /// <returns>Успешность действия</returns>
        [HttpPost]
        [Route("processing-order")]
        public async Task<IResult> DoubleOrder([FromBody] OperatorProcessingOrderInputModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Access denied."); // Учётная запись оператора не одобрена администратором

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Access denied");

                if (!ACTIONS.Contains(model.Action)) throw new Exception("Access denied."); // Идентификатор действия находится в промежутке между 0 и 2.
                
                var result = await _operatorService.ProcessingOrderAsync(model, GetUserName());
                
                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
