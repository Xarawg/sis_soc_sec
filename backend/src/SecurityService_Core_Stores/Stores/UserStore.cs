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

        private readonly DbSet<Docscan> Docscans;
        private readonly DbSet<Order> Orders;
        private readonly DbSet<User> Users;
        private readonly DbSet<UserHash> UserHashes;

        public UserStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<Docscan>();
            Orders = customerContext.Set<Order>();
            Users = customerContext.Set<User>();
            UserHashes = customerContext.Set<UserHash>();
        }
        public async Task<List<User>> GetUsersAsync() => await Users.AsNoTracking().ToListAsync();
        public async Task<User?> CheckUserByLoginAsync(string login) => await Users.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<User?> GetUserByLoginAsync(string login) => await Users.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<UserHash?> GetUserHashAsync(string login) => await UserHashes.AsNoTracking().Where(x => x.UserName == login).FirstOrDefaultAsync();
        public async Task<UserHash?> GetExistsUserHashAsync(byte[] hash) => await UserHashes.AsNoTracking().Where(x => x.Hash!.Equals(hash)).FirstOrDefaultAsync();

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <param name="user"></param>
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
                    var newUser = new User()
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
                        UserRole = adminModel != null ? adminModel.Role : (int)UserRole.Operator,
                        FIO = userModel != null ? userModel.FIO : adminModel.FIO,
                        Organization = userModel != null ? userModel.Organization : adminModel.Organization,
                        INN = userModel != null ? userModel.INN : adminModel.INN,
                        Address = userModel != null ? userModel.Address : adminModel.Address,
                        //Status = adminModel != null ? adminModel.Status : (int)UserStatus.New, // TODO: Временно все получают статус Registered для тестирования
                        Status = adminModel != null ? adminModel.Status : (int)UserStatus.Registered,
                        CreateDate = DateTime.UtcNow,
                        CreateUser = userName != null ? userName : ""
                    };

                    await Users.AddAsync(newUser);

                    var newHash = new UserHash()
                    {
                        IdUser = idUser,
                        Hash = hash,
                        Salt = salt,
                        Status = (int)UserStatus.New,
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
        public async Task UpdateUser(User user)
        {
            try
            {
                var userDB = await Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                userDB = user;
                _customerContext.Update(userDB);
                await _customerContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
