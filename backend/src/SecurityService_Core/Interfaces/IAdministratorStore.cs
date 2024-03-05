
namespace SecurityService_Core.Interfaces
{
    public interface IAdministratorStore
    {
        Task BlockUserAsync(Guid id, string userName);
        Task DeclineUserAsync(Guid id, string userName);
        Task RegisterUserAsync(Guid id, string userName);
    }
}
