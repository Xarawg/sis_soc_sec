using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.Enums;

namespace SecurityService_Core_Stores.Stores
{
    public class OperatorStore : IOperatorStore
    {
        private readonly CustomerContext _customerContext;

        private readonly DbSet<DocscanDB> Docscans;
        private readonly DbSet<OrderDB> Orders;
        private readonly DbSet<OrderStatusDB> OrderStatuses;
        private readonly DbSet<UserDB> Users;

        public OperatorStore(CustomerContext customerContext)
        {
            _customerContext = customerContext;

            Docscans = customerContext.Set<DocscanDB>();
            Orders = customerContext.Set<OrderDB>();
            OrderStatuses = customerContext.Set<OrderStatusDB>();
            Users = customerContext.Set<UserDB>();
        }

        public async Task<List<OrderDB>> GetOrdersAsync(OperatorOrderGetModel model)
        {
            var orders = Orders.AsNoTracking().AsQueryable();
            if (model.LimitOffset != null && model.LimitOffset != 0)
            {
                orders = orders.Skip((int)model.LimitOffset);
            }
            if (model.LimitRowCount != null && model.LimitRowCount != 0)
            {
                orders = orders.Take((int)model.LimitRowCount);
            }
            return await orders.ToListAsync();
        }

        public async Task<DocscanDB> GetDocscanAsync(Guid idDoc) => await Docscans.AsNoTracking().Where(x => x.Id == idDoc).FirstOrDefaultAsync();
        public async Task<List<DocscanDB>> GetDocscansAsync(Guid idOrder) => await Docscans.AsNoTracking().Where(x => x.IdOrder == idOrder).ToListAsync();
        public async Task<List<DocscanDB>> GetDocscanListAsync(List<Guid> idOrders)
        {
            return await Docscans.AsNoTracking().Where(x => idOrders.Contains(x.IdOrder)).ToListAsync();
        }
        public async Task<Dictionary<int, string>> GetOrderStatusesAsync() => await OrderStatuses.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.OrderStatusName);

        /// <summary>
        /// Создание заявки
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrderAsync(Guid idOrder, OperatorOrderInputModel model, string userName, List<DocscanDB> docs)
        {
            var newOrder = new OrderDB()
            {
                Id = idOrder,
                Date = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow,
                CreateUser = userName,
                State = (int)OrderStatus.New,
                Status = "",
                SNILS = model.SNILS,
                FIO = model.FIO,
                ContactData = model.ContactData,
                Type = model.Type
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
            if ((OrderStatus)order.State! != OrderStatus.New)
                throw new Exception("Отменить можно только новую заявку.");

            order.State = 1;
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
            if ((OrderStatus)order.State! != OrderStatus.New
                    && (OrderStatus)order.State! != OrderStatus.Registered)
                throw new Exception("Отменить можно только новую и зарегистрированную заявки.");

            order.State = -2;
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
            order.Id = Guid.NewGuid();
            order.CreateDate = DateTime.UtcNow;
            order.CreateUser = userName;

            await Orders.AddAsync(order);
            await _customerContext.SaveChangesAsync();

            return true;
        }

    }
}
