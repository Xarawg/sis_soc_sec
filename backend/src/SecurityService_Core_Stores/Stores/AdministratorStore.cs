using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.Enums;

namespace SecurityService_Core_Stores.Stores
{
    public class AdministratorStore : IAdministratorStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<DocscanDB> Docscans;
        private readonly DbSet<OrderDB> Orders;
        private readonly DbSet<UserDB> Users;
        private readonly DbSet<UserStatusDB> UserStatuses;

        public AdministratorStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<DocscanDB>();
            Orders = customerContext.Set<OrderDB>();
            Users = customerContext.Set<UserDB>();
            UserStatuses = customerContext.Set<UserStatusDB>();
        }

        public async Task BlockUserAsync(Guid idUser, string userName)
        {
            var statusesDictionary = await UserStatuses.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.UserStatusName);
            var user = await Users.FirstOrDefaultAsync(x => x.Id == idUser);
            if (user == null) throw new Exception("Пользователя не существует.");
            var dateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            user.ChangeDate = dateNow;
            user.ChangeUser = userName;
            user.State = (int)UserStatus.Blocked;
            user.Status = statusesDictionary.FirstOrDefault(x => x.Key == (int)UserStatus.Blocked).Value;

            Users.Update(user);
            await _customerContext.SaveChangesAsync();
        }

        public async Task DeclineUserAsync(Guid idUser, string userName)
        {
            var statusesDictionary = await UserStatuses.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.UserStatusName);
            var user = await Users.FirstOrDefaultAsync(x => x.Id == idUser);
            if (user == null) throw new Exception("Пользователя не существует.");
            var dateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            user.ChangeDate = dateNow;
            user.ChangeUser = userName;
            user.State = (int)UserStatus.Declined;
            user.Status = statusesDictionary.FirstOrDefault(x => x.Key == (int)UserStatus.Declined).Value;

            Users.Update(user);
            await _customerContext.SaveChangesAsync();
        }

        public async Task RegisterUserAsync(Guid idUser, string userName)
        {
            var statusesDictionary = await UserStatuses.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.UserStatusName);
            var user = await Users.FirstOrDefaultAsync(x => x.Id == idUser);
            if (user == null) throw new Exception("Пользователя не существует.");
            var dateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            user.CreateDate = dateNow;
            user.CreateUser = userName;
            user.State = (int)UserStatus.Registered;
            user.Status = statusesDictionary.FirstOrDefault(x => x.Key == (int)UserStatus.Registered).Value;

            Users.Update(user);
            await _customerContext.SaveChangesAsync();
        }
    }
}
