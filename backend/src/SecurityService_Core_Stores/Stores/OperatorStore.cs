using Microsoft.EntityFrameworkCore;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;
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
            var dateNow = DateTime.UtcNow;
            var newOrder = new OrderDB()
            {
                Id = idOrder,
                Date = dateNow,
                CreateDate = dateNow,
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
            var order = await Orders.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (order == null) throw new Exception("Заявки не существует.");
            var dateNow = DateTime.UtcNow;
            order.CreateDate = dateNow;
            order.CreateUser = userName;
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
            var order = await Orders.FirstOrDefaultAsync(x => x.Id == idOrder);
            if (order == null) throw new Exception("Заявки не существует.");
            if ((OrderStatus)order.State! != OrderStatus.New)
                throw new Exception("Отменить можно только новую заявку.");

            var dateNow = DateTime.UtcNow;
            order.ChangeDate = dateNow;
            order.State = (int)OrderStatus.Registered;
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
            var order = await Orders.FirstOrDefaultAsync(x => x.Id == idOrder);
            if (order == null) throw new Exception("Заявки не существует.");
            if ((OrderStatus)order.State! != OrderStatus.New
                    && (OrderStatus)order.State! != OrderStatus.Registered)
                throw new Exception("Отменить можно только новую и зарегистрированную заявки.");

            var dateNow = DateTime.UtcNow;
            order.ChangeDate = dateNow;
            order.State = -2;
            order.ChangeUser = userName;
            order.State = (int)OrderStatus.Declined;

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
            try
            {
                var order = await Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idOrder);
                if (order == null) throw new Exception("Заявки не существует.");
                var dateNow = DateTime.UtcNow;
                order.Date = dateNow;
                order.CreateDate = dateNow;
                order.Id = Guid.NewGuid();
                order.CreateUser = userName;
                order.State = (int)OrderStatus.New;

                var statuses = await GetOrderStatusesAsync();
                order.Status = statuses.FirstOrDefault(x => x.Key == (int)OrderStatus.New).Value;

                await Orders.AddAsync(order);

                var docs = await Docscans.AsNoTracking().Where(x => x.IdOrder == idOrder).ToListAsync();
                foreach (var doc in docs)
                {
                    doc.Id = Guid.NewGuid();
                    doc.IdOrder = order.Id;
                    doc.CreateDate = dateNow;
                    doc.CreateUser = userName;
                }
                await Docscans.AddRangeAsync(docs);

                await _customerContext.SaveChangesAsync();

                return true;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
