using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace SecurityService_Core_Stores.Stores
{
    public class OperatorStore : IOperatorStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<Docscan> Docscans;
        private readonly DbSet<Order> Orders;
        private readonly DbSet<User> Users;

        public OperatorStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<Docscan>();
            Orders = customerContext.Set<Order>();
            Users = customerContext.Set<User>();
        }

        public async Task<List<Order>> GetOrdersAsync() => await Orders.AsNoTracking().ToListAsync();

        /// <summary>
        /// Создание заявки
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrderAsync(Guid idOrder, OperatorOrderInputModel model, string userName, List<Docscan> docs)
        {
            var newOrder = new Order()
            {
                Id = idOrder,
                Date = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow,
                CreateUser = userName
            };
            await Orders.AddAsync(newOrder);
            await Docscans.AddRangeAsync(docs);
            _customerContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Создание заявки
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> ChangeOrderAsync(OperatorChangeOrderInputModel model, string userName)
        {
            var order = await Orders.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            order.SNILS = model.SNILS;
            order.FIO = model.FIO;
            order.ContactData = model.ContactData;
            order.Type = model.Type;
            order.SupportMeasures = model.SupportMeasures;
            Orders.Update(order);
            _customerContext.SaveChanges();

            return true;
        }

    }
}
