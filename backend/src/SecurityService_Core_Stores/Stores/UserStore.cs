using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.Enums;

namespace SecurityService_Core_Stores.Stores
{
    public class UserStore : IUserStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<DocscanDB> Docscans;
        private readonly DbSet<OrderDB> Orders;
        private readonly DbSet<UserDB> Users;
        private readonly DbSet<UserRoleDB> UserRoles;
        private readonly DbSet<UserStatusDB> UserStatuses;
        private readonly DbSet<UserHashDB> UserHashes;

        public UserStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<DocscanDB>();
            Orders = customerContext.Set<OrderDB>();
            Users = customerContext.Set<UserDB>();
            UserRoles = customerContext.Set<UserRoleDB>();
            UserStatuses = customerContext.Set<UserStatusDB>();
            UserHashes = customerContext.Set<UserHashDB>();
        }
        public async Task<List<UserDB>> GetUsersAsync() => await Users.AsNoTracking().ToListAsync();
        public async Task<Dictionary<int, string>> GetUserRolesAsync() => await UserRoles.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.UserRoleName);
        public async Task<Dictionary<int, string>> GetUserStatusesAsync() => await UserStatuses.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.UserStatusName);
        public async Task<UserDB?> CheckUserByLoginAsync(string login) => await Users.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<UserDB?> GetUserByLoginAsync(string login) => await Users.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<UserHashDB?> GetUserHashAsync(string login) => await UserHashes.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<UserHashDB?> GetExistsUserHashAsync(byte[] hash) => await UserHashes.AsNoTracking().Where(x => x.Hash!.Equals(hash)).FirstOrDefaultAsync();

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <param name="userName"></param>
        /// <param name="userModel"></param>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InsertUserAsync(Guid idUser, string salt, byte[] hash, string? userName, UserRegistrationInputModel? userModel = null, AdminRegistrationInputModel? adminModel = null)
        {
            try
            {
                if (userModel != null || adminModel != null)
                {
                    var newUser = new UserDB()
                    {
                        Id = idUser,
                        UserName = userModel != null ? userModel.UserName : adminModel.UserName,
                        Email = userModel != null ? userModel.Email : adminModel.Email,
                        EmailConfirmed = true, // TODO не реализовано
                        PasswordHash = hash,
                        PhoneNumber = userModel != null ? userModel.PhoneNumber : adminModel.PhoneNumber,
                        PhoneNumberConfirmed = true, // TODO не реализовано
                        TwoFactorEnabled = true, // TODO не реализовано
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        UserRole = adminModel != null ? adminModel.Role : (int)UserRole.None,
                        FIO = userModel != null ? userModel.FIO : adminModel.FIO,
                        Organization = userModel != null ? userModel.Organization : adminModel.Organization,
                        INN = userModel != null ? userModel.INN : adminModel.INN,
                        Address = userModel != null ? userModel.Address : adminModel.Address,
                        Status = adminModel != null ? adminModel.Status : (int)UserStatus.New,
                        CreateDate = DateTime.UtcNow,
                        CreateUser = userName != null ? userName : ""
                    };

                    await Users.AddAsync(newUser);

                    var newHash = new UserHashDB()
                    {
                        IdUser = idUser,
                        Hash = hash,
                        Salt = salt,
                        Status = adminModel != null ? adminModel.Status : (int)UserStatus.New,
                        UserName = newUser.UserName,
                    };
                    await UserHashes.AddAsync(newHash);

                    _customerContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateUserAsync(UserDB user)
        {
            try
            {
                // Получаем пользователя из БД
                var userDB = await Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
                // Получаем хэш пользователя из БД
                var userHashDB = await UserHashes.Where(u => u.IdUser == user.Id).FirstOrDefaultAsync();
                if (userDB != null && userHashDB != null)
                {
                    userDB.AccessFailedCount = user.AccessFailedCount;
                    userDB.LockoutEnabled = user.LockoutEnabled;
                    userDB.AccessFailedAttemptDate = user.AccessFailedAttemptDate;
                    Users.Update(userDB);

                    userHashDB.Status = user.Status;
                    UserHashes.Update(userHashDB);
                    await _customerContext.SaveChangesAsync();
                } else
                {
                    throw new Exception("Пользователь отсутствует в БД");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Создание временного пароля пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateTemporaryUserPasswordAsync(ChangePasswordDTO model, string salt, byte[] hash, int temporaryTime)
        {
            try
            {
                var userDB = await Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                var hashDB = await UserHashes.FirstOrDefaultAsync(u => u.UserName == model.UserName);

                userDB!.IsTemporaryAccess = true;
                userDB!.TemporaryAccessExpirationTime = DateTime.UtcNow.AddSeconds(temporaryTime);
                Users.Update(userDB);

                hashDB!.Hash = hash;
                hashDB!.Salt = salt;
                UserHashes.Update(hashDB);
                await _customerContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ChangePasswordAsync(ChangePasswordDTO model, string salt, byte[] hash)
        {
            try
            {
                var userDB = await Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                var hashDB = await UserHashes.FirstOrDefaultAsync(u => u.UserName == model.UserName);

                userDB!.IsTemporaryAccess = false;
                userDB!.TemporaryAccessExpirationTime = null;
                Users.Update(userDB);

                hashDB!.Hash = hash;
                hashDB!.Salt = salt;
                UserHashes.Update(hashDB);
                await _customerContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
