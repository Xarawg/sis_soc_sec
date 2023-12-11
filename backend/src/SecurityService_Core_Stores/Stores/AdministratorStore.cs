using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores
{
    public class AdministratorStore : IAdministratorStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<DocscanDB> Docscans;
        private readonly DbSet<OrderDB> Orders;
        private readonly DbSet<UserDB> Users;

        public AdministratorStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<DocscanDB>();
            Orders = customerContext.Set<OrderDB>();
            Users = customerContext.Set<UserDB>();
        }

    }
}
