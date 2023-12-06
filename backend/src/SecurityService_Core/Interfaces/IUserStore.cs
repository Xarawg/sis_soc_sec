using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;

namespace SecurityService_Core.Interfaces
{
    public interface IUserStore
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> CheckUserByLoginAsync(string login);
        Task<User?> GetUserByLoginAsync(string login);
        Task<UserHash?> GetUserHashAsync(string login);
        Task<UserHash?> GetExistsUserHashAsync(byte[] hash);
        Task InsertUserAsync(Guid idUser, string salt, byte[] hash, string? user = null, UserRegistrationInputModel? model = null, AdminRegistrationInputModel? adminModel = null);
        Task UpdateUser(User user);
    }
}
