using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.Enums;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/administrator")]
    public class AdministratorController : BaseController
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly AdministratorService _administratorService;
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="administratorService"></param>
        /// <param name="userService"></param>
        /// <param name="httpContextAccessor"></param>
        public AdministratorController(
            ILogger<AdministratorController> logger
            , AdministratorService administratorService
            , UserService userService
            ,IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _administratorService = administratorService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Зарегистрировать пользователя средствами администратора
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Сейчас регистрирует всех администраторов со статусом 1 - т.е. все сразу одобрены для работы с данными. </remarks>
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IResult> Register([FromBody] AdminRegistrationInputModel model)
        {
            try
            {
                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator)
                {
                    throw new Exception("Недостаточно прав");
                }
                if (Enumerable.Range(0, 1).Contains(model.Role)) throw new Exception("Идентификатор роли находится в промежутке между 0 и 1.");
                if (Enumerable.Range(-2, 1).Contains(model.Status)) throw new Exception("Идентификатор статуса находится в промежутке между -2 и 1.");
                var result = await _userService.RegisterAsync(null, model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Изменить пользователя средствами администратора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-user")]
        public async Task<IResult> ChangeUser([FromBody] AdminRegistrationInputModel model)
        {
            try
            {
                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator)
                {
                    throw new Exception("Недостаточно прав");
                }
                if (Enumerable.Range(0, 1).Contains(model.Role)) throw new Exception("Идентификатор роли находится в промежутке между 0 и 1.");
                if (Enumerable.Range(-2, 1).Contains(model.Status)) throw new Exception("Идентификатор статуса находится в промежутке между -2 и 1.");
                var result = await _administratorService.ChangeUserAsync(model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Изменить пароль другого пользователя средствами администратора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        public async Task<IResult> ChangePassword([FromBody] AdminChangePasswordDTO model)
        {
            try
            {
                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator)
                {
                    throw new Exception("Недостаточно прав");
                }
                var result = await _administratorService.ChangePasswordAsync(model);

                return Results.Json(Result<AdminChangePasswordDTO>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Получить список пользователей администратором
        /// </summary>
        /// <returns>Список пользователей</returns>
        [HttpGet]
        [Route("get-users")]
        public async Task<IResult> GetUsers()
        {
            try
            {
                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator)
                {
                    throw new Exception("Недостаточно прав");
                }
                var result = await _userService.GetUsersAsync();

                return Results.Json(Result<List<UserDTO>>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
