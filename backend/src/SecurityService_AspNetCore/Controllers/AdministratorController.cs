using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.ControllerDTO.User;
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

        public readonly List<int> USER_ROLES = Enum.GetValues(typeof(UserRole)).Cast<int>().ToList();
        public readonly List<int> USER_STATUSES = Enum.GetValues(typeof(UserStatus)).Cast<int>().ToList();
        public readonly List<int> ACTIONS = Enum.GetValues(typeof(UserProcessingAction)).Cast<int>().ToList();

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
            , IHttpContextAccessor httpContextAccessor
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
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        [Route("create-new-user")]
        public async Task<IResult> Register([FromBody] AdminRegistrationInputModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Учётная запись оператора не одобрена администратором.");

                if (model.INN.Length != 10 && model.INN.Length != 12) throw new Exception("ИНН у физических лиц составляет в длину 12 символов, у юридических - 10 символов.");
                if (!USER_ROLES.Contains(model.Role)) throw new Exception("Идентификатор роли находится в промежутке между 0 и 31.");
                if (!USER_STATUSES.Contains(model.State)) throw new Exception("Идентификатор статуса находится в промежутке между -2 и 1.");

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Недостаточно прав.");
                if (model.Role == (int)UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Выдать роль администратора может только супер администратор.");
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
        public async Task<IResult> ChangeUser([FromBody] AdminChangeInputModel model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Учётная запись не одобрена администратором.");

                if (!USER_ROLES.Contains(model.Role)) throw new Exception("Идентификатор роли находится в промежутке между 0 и 1.");
                if (!USER_STATUSES.Contains(model.State)) throw new Exception("Идентификатор статуса находится в промежутке между -2 и 1.");

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Недостаточно прав.");
                if (model.Role == (int)UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Выдать роль администратора может только супер администратор.");
                var result = await _userService.ChangeUserAsync(model);

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
        [Route("change-user-password")]
        public async Task<IResult> ChangeUserPassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Учётная запись не одобрена администратором.");

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Недостаточно прав.");

                var userWhoHasHadHisRoleChanged = (UserRole?)await _userService.GetUserRoleByLoginAsync(model.UserName);
                if (
                    userWhoHasHadHisRoleChanged == UserRole.SuperAdministrator
                    || (userWhoHasHadHisRoleChanged == UserRole.Administrator && userRole == UserRole.Administrator)
                    )
                {
                    throw new Exception("Запрещено менять пароль этому пользователю.");
                }
                var result = await _userService.CreateTemporaryUserPasswordAsync(model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
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
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Учётная запись не одобрена администратором.");

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Administrator && userRole != UserRole.SuperAdministrator) throw new Exception("Недостаточно прав.");
                var result = await _userService.GetUsersAsync();

                return Results.Json(Result<List<UserDTO>>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Присвоить учётной записи пользователя статус на выбор.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Успешность действия</returns>
        [HttpPost]
        [Route("processing-user")]
        public async Task<IResult> ProcessingUser([FromBody] AdministratorProcessingUserInputModel model)
        {
            try
            {
                if (model.Id == Guid.Empty) throw new Exception("Пользователя не существует.");
                var userStatus = (UserStatus?)await _userService.GetUserStatusByLoginAsync(GetUserName());
                if (userStatus != UserStatus.Registered) throw new Exception("Учётная запись оператора не одобрена администратором.");

                var userRole = (UserRole?)await _userService.GetUserRoleByLoginAsync(GetUserName());
                if (userRole != UserRole.Operator && userRole != UserRole.SuperAdministrator) throw new Exception("Недостаточно прав");

                if (!ACTIONS.Contains(model.Action)) throw new Exception("Идентификатор действия находится в промежутке между 0 и 2.");

                var result = await _administratorService.ProcessingUserAsync(model, GetUserName());

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
