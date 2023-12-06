using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core.Interfaces
{
    public interface IOperatorStore
    {
        Task<List<Order>> GetOrdersAsync();
        Task<bool> CreateOrderAsync(Guid idOrder, OperatorOrderInputModel model, string userName, List<Docscan> docs);
        Task<bool> ChangeOrderAsync(OperatorChangeOrderInputModel model, string userName);
        Task<bool> SendOrderAsync(Guid idOrder, string userName);
        Task<bool> DeclineOrderAsync(Guid idOrder, string userName);
        Task<bool> DoubleOrderAsync(Guid idOrder, string userName);
    }
}
