using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core.Interfaces
{
    public interface IOperatorStore
    {
        Task<List<OrderDB>> GetOrdersAsync(OperatorOrderGetModel model);
        Task<List<DocscanDB>> GetDocscansAsync(Guid idOrder);
        Task<DocscanDB> GetDocscanAsync(Guid idDoc);
        Task<List<DocscanDB>> GetDocscanListAsync(List<Guid> idOrder);
        Task<Dictionary<int, string>> GetOrderStatusesAsync();
        Task<bool> CreateOrderAsync(Guid idOrder, OperatorOrderInputModel model, string userName, List<DocscanDB> docs);
        Task<bool> ChangeOrderAsync(OperatorChangeOrderInputModel model, string userName);
        Task<bool> SendOrderAsync(Guid idOrder, string userName);
        Task<bool> DeclineOrderAsync(Guid idOrder, string userName);
        Task<bool> DoubleOrderAsync(Guid idOrder, string userName);
    }
}
