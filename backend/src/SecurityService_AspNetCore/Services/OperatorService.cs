using AutoMapper;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.DB;
using SecurityService_Core_Stores.Stores;
using SecurityService_Core.Models.Enums;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class OperatorService
    {
        private readonly IMapper _mapper;
        private readonly IOperatorStore _operatorStore;

        /// <summary>
        /// 5 МБ ограничение загружаемого файла
        /// </summary>
        private const int FILE_SIZE_LIMIT = 5240000;

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="operatorStore"></param>
        public OperatorService(IMapper mapper, IOperatorStore operatorStore)
        {
            _mapper = mapper;
            _operatorStore = operatorStore;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(OperatorOrderGetModel model)
        {
            var orders = await _operatorStore.GetOrdersAsync(model);
            var orderStatuses = await _operatorStore.GetOrderStatusesAsync();
            if (orders == null)
            {
                throw new Exception("Заявки не найдены");
            }
            var result = _mapper.Map<IEnumerable<OrderDB>, IEnumerable<OrderDTO>>(orders).ToList();
            var resultIdOrders = result.Select(x => x.Id).ToList();
            var docsDB = await _operatorStore.GetDocscanListAsync(resultIdOrders);
            var docs = _mapper.Map<IEnumerable<DocscanDB>, IEnumerable<DocscanWithoutFileBodyDTO>>(docsDB).ToList();

            result.ForEach(o =>
            {
                o.Status = orderStatuses[int.Parse(o.Status)];
                o.Documents = docs.Where(x => x.IdOrder == o.Id).ToList();
            });
            return result;
        }

        public async Task<DocscanDTO> GetDocscanAsync(Guid idDoc)
        {
            try
            {
                var doc = await _operatorStore.GetDocscanAsync(idDoc);
                if (doc == null) throw new Exception("Документ не найден");
                var result = _mapper.Map<DocscanDB, DocscanDTO>(doc);
                return result;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Создание заявки
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CreateOrderAsync(OperatorOrderInputModel model, string userName)
        {
            try
            {
                var idOrder = Guid.NewGuid();
                List<DocscanDB> docs = new List<DocscanDB>();
                foreach (var doc in model.Documents)
                {
                    // Получаем массив байт файла из IFormFile
                    var data = await GetBytesAsync(doc);
                    // Проверяем условия размера файла
                    if (data.Length > FILE_SIZE_LIMIT) throw new Exception($"Превышен лимит размера 5 МБ у файла {doc.FileName}.");
                    docs.Add(new DocscanDB()
                    {
                        Id = Guid.NewGuid(),
                        IdOrder = idOrder,
                        FileBody = data,
                        FileName = MakeValidFileName(doc.FileName),
                        FileExt = Path.GetExtension(doc.FileName)
                    });
                }
                return await _operatorStore.CreateOrderAsync(idOrder, model, userName, docs);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangeOrderAsync(OperatorChangeOrderInputModel model, string userName)
        {
            try
            {
                return await _operatorStore.ChangeOrderAsync(model, userName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ProcessingOrderAsync(OperatorProcessingOrderInputModel model, string userName)
        {
            try
            {
                switch ((OrderProcessingAction)model.Action)
                {
                    case OrderProcessingAction.Send:
                        await _operatorStore.SendOrderAsync(model.Id, userName);
                        break;
                    case OrderProcessingAction.Decline:
                        await _operatorStore.DeclineOrderAsync(model.Id, userName);
                        break;
                    case OrderProcessingAction.Double:
                        await _operatorStore.DoubleOrderAsync(model.Id, userName);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Служебные методы

        /// <summary>
        /// Обезопасить название файла
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string MakeValidFileName(string? name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name ?? string.Empty, invalidRegStr, "_");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private static async Task<byte[]> GetBytesAsync(IFormFile? formFile)
        {
            await using var memoryStream = new MemoryStream();
            if (formFile != null)
            {
                await formFile.CopyToAsync(memoryStream);
            }
            return memoryStream.ToArray();
        }
        #endregion

    }
}
