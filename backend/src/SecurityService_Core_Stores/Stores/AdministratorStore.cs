using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores
{
    public class AdministratorStore : IAdministratorStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<Docscan> Docscans;
        private readonly DbSet<Order> Orders;
        private readonly DbSet<User> Users;

        public AdministratorStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<Docscan>();
            Orders = customerContext.Set<Order>();
            Users = customerContext.Set<User>();
        }

        public async Task<List<User>> GetUsersAsync() => await Users.AsNoTracking().ToListAsync();
    }
}
