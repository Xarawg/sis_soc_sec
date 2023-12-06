using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.Enums;
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
                CreateUser = userName,
                Status = (int)OrderStatus.New,
                SNILS = model.SNILS,
                FIO = model.FIO,
                ContactData = model.ContactData,
                Type = model.Type,
                SupportMeasures = model.SupportMeasures
            };

            await Orders.AddAsync(newOrder);
            await Docscans.AddRangeAsync(docs);
            await _customerContext.SaveChangesAsync();
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
            if (order == null) throw new Exception("Заявки не существует.");
            order.SNILS = model.SNILS;
            order.FIO = model.FIO;
            order.ContactData = model.ContactData;
            order.Type = model.Type;
            order.SupportMeasures = model.SupportMeasures;
            Orders.Update(order);
            await _customerContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Отправка заявки на обработку (зарегистрировать её)
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> SendOrderAsync(Guid idOrder, string userName)
        {
            var order = await Orders.Where(x => x.Id == idOrder).FirstOrDefaultAsync();
            if (order == null) throw new Exception("Заявки не существует.");
            if ((OrderStatus)order.Status! != OrderStatus.New)
                throw new Exception("Отменить можно только новую заявку.");

            order.Status = 1;
            order.ChangeDate = DateTime.UtcNow;
            order.ChangeUser = userName;

            Orders.Update(order);
            await _customerContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Отмена заявки
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> DeclineOrderAsync(Guid idOrder, string userName)
        {
            var order = await Orders.Where(x => x.Id == idOrder).FirstOrDefaultAsync();
            if (order == null) throw new Exception("Заявки не существует.");
            if ((OrderStatus)order.Status! != OrderStatus.New
                    && (OrderStatus)order.Status! != OrderStatus.Registered)
                throw new Exception("Отменить можно только новую и зарегистрированную заявки.");

            order.Status = -2;
            order.ChangeDate = DateTime.UtcNow;
            order.ChangeUser = userName;

            Orders.Update(order);
            await _customerContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Дублирование заявки
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> DoubleOrderAsync(Guid idOrder, string userName)
        {
            var order = await Orders.AsNoTracking().Where(x => x.Id == idOrder).FirstOrDefaultAsync();
            if (order == null) throw new Exception("Заявки не существует.");
            order.Id = idOrder;
            order.CreateDate = DateTime.UtcNow;
            order.CreateUser = userName;

            await Orders.AddAsync(order);
            await _customerContext.SaveChangesAsync();

            return true;
        }

    }
}
