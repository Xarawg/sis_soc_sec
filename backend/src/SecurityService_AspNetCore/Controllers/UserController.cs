using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.User;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
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
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IResult> Register([FromBody] UserRegistrationDTO model)
        {
            try
            {
                var result = await _userService.RegisterAsync(model);

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
        [Route("change-password")]
        public async Task<IResult> ChangePassword([FromBody] UserChangePasswordDTO model)
        {
            try
            {
                var result = await _userService.ChangePasswordAsync(model);

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
        [Route("auth")]
        public async Task<IResult> Auth([FromBody] UserAuthDTO model)
        {
            try
            {
                var result = await _userService.AuthAsync(model);

                return Results.Json(Result<bool>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
