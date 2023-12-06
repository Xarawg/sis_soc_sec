using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;

namespace Security_Service_AspNetCore.Controllers
{
    // <summary>
    /// Контроллер обработки файлов и связанных сущностей
    /// </summary>
    [ApiController]
    [Route("api/administrator")]
    public class AdministratorController : BaseController
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly AdministratorService _administratorService;

        /// <summary>
        /// Конструктор контроллера файлов
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="logService"></param>
        /// <param name="administratorService"></param>
        public AdministratorController(ILogger<AdministratorController> logger, AdministratorService administratorService)
        {
            _logger = logger;
            _administratorService = administratorService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IResult> Register([FromBody] AdminRegistrationDTO model)
        {
            try
            {
                var result = await _administratorService.RegisterAsync(model);

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
        [Route("change-user")]
        public async Task<IResult> ChangeUser([FromBody] AdminRegistrationDTO model)
        {
            try
            {
                var result = await _administratorService.ChangeUserAsync(model);

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
        /// <returns>Список файлов</returns>
        [HttpPost]
        [Route("change-password")]
        public async Task<IResult> ChangePassword([FromBody] AdminChangePasswordDTO model)
        {
            try
            {
                var result = await _administratorService.ChangePasswordAsync(model);

                return Results.Json(Result<AdminChangePasswordDTO>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Список файлов</returns>
        [HttpGet]
        [Route("get-users")]
        public async Task<IResult> GetUsers()
        {
            try
            {
                var result = await _administratorService.GetUsersAsync();

                return Results.Json(Result<List<UserDTO>>.CreateSuccess(result), serializerOptions);
            }
            catch (Exception ex)
            {
                return Results.Json(Result<string>.CreateFailure(ex.Message), serializerOptions);
            }
        }
    }
}
