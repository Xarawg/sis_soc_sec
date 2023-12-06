using SecurityService_Core.Models.DB;

namespace SecurityService_Core.Interfaces
{
    public interface IOperatorStore
    {
        /// <summary>
        /// Получение данных о погоде для изменения
        /// </summary>
        /// <returns>Список данных о погоде</returns>
        Task<List<Order>> GetOrdersAsync();
    }
}
