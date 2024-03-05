using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_AspNetCore.Services.Communication;
using SecurityService_Core.Models.ControllerDTO.User;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;

        /// <summary>
        /// Конструктор контроллера файлов
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Регистрация пользователя через страницу регистрации на форме входа.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Для тестирования все регистрируются с одобренной учётной записью под админкой </remarks>
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IResult> Register([FromBody] UserRegistrationInputModel model)
        {
            try
            {
                if (model.INN.Length != 10 && model.INN.Length != 12) throw new Exception("ИНН у физических лиц составляет в длину 12 символов, у юридических - 10 символов."); 
                var result = await _userService.RegisterAsync(model);
                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Аутентификация и получение токена
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Результат аутентификации</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("auth")]
        public async Task<IResult> Auth([FromBody] UserAuthInputModel model)
        {
            try
            {
                var result = await _userService.AuthAsync(model);

                return Results.Json(Result<TokenResponse>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// Изменение собственного пароля пользователем
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Результат аутентификации</returns>
        [HttpPost]
        [Route("change-password")]
        public async Task<IResult> ChangePassword([FromBody] UserChangePasswordModel model)
        {
            try
            {
                var result = await _userService.ChangePasswordAsync(new ChangePasswordDTO() { UserName = GetUserName(), Password = model.Password });

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
