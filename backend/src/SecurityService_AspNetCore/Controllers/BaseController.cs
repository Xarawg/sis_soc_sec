using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Security_Service_AspNetCore.Controllers
{
    /// <summary>
    /// Базовый контроллер конфигурации и методов общего назначения
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Опции сериализации для методов, возвращающих JSON
        /// </summary>
        protected JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web);

        /// <summary>
        /// Признак аутентифицированного пользователя
        /// </summary>
        public bool IsAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// IP адрес пользователя
        /// </summary>
        public string IpAddress => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP не найден";

        /// <summary>
        /// Конструктор базового контроллера
        /// </summary>
        protected BaseController()
        {
            // Исключение цикличности моделей при сериализации
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        /// <summary>
        /// Возвращает имя пользователя из http контекста
        /// </summary>
        /// <returns>Имя пользователя</returns>
        /// <exception cref="Exception">
        ///     Пользователь не аутентифицирован
        /// </exception>
        protected string GetUserName()
        {
            try
            {
                if (!IsAuthenticated)
                {
                    throw new Exception("Пользователь не аутентифицирован");
                }
                return HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Login")?.Value ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Возвращает ID пользователя из http контекста
        /// </summary>
        /// <returns>ID пользователя</returns>
        /// <exception cref="Exception">
        ///     Пользователь не аутентифицирован
        /// </exception>
        protected Guid GetUserId()
        {
            try
            {
                if (!IsAuthenticated)
                {
                    throw new Exception("Пользователь не аутентифицирован");
                }
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Пользователь не аутентифицирован");
                }
                return new Guid(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}