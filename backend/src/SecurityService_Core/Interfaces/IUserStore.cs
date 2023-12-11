using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;

namespace SecurityService_Core.Interfaces
{
    public interface IUserStore
    {
        Task<List<UserDB>> GetUsersAsync();
        Task<Dictionary<int, string>> GetUserRolesAsync();
        Task<Dictionary<int, string>> GetUserStatusesAsync();
        Task<UserDB?> CheckUserByLoginAsync(string login);
        Task<UserDB?> GetUserByLoginAsync(string login);
        Task<UserHashDB?> GetUserHashAsync(string login);
        Task<UserHashDB?> GetExistsUserHashAsync(byte[] hash);
        Task InsertUserAsync(Guid idUser, string salt, byte[] hash, string? user = null, UserRegistrationInputModel? model = null, AdminRegistrationInputModel? adminModel = null);
        Task UpdateUserAsync(UserDB user);
        Task CreateTemporaryUserPasswordAsync(ChangePasswordDTO model, string salt, byte[] hash, int temporaryTime);
        Task ChangePasswordAsync(ChangePasswordDTO model, string salt, byte[] hash);
    }
}
