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
        /// Конструктор базового контроллера
        /// </summary>
        protected BaseController()
        {
            // Исключение цикличности моделей при сериализации
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }
    }
}