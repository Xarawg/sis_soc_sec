using AutoMapper;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.Operator;
using SecurityService_Core.Models.Enums;
using SecurityService_Core_Stores.Stores;

namespace Security_Service_AspNetCore.Services
{
    /// <summary>
    /// Сервис обработки файлов и связанных сущностей
    /// </summary>
    public class AdministratorService
    {
        private readonly IMapper _mapper;
        private readonly IAdministratorStore _administratorStore;

        /// <summary>
        /// Конструктор сервиса файлов
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="administratorStore"></param>
        public AdministratorService(IMapper mapper, IAdministratorStore administratorStore)
        {
            _mapper = mapper;
            _administratorStore = administratorStore;
        }

        public async Task<bool> ProcessingUserAsync(AdministratorProcessingUserInputModel model, string userName)
        {
            try
            {
                switch ((UserProcessingAction)model.Action)
                {
                    case UserProcessingAction.Block:
                        await _administratorStore.BlockUserAsync(model.Id, userName);
                        break;
                    case UserProcessingAction.Register:
                        await _administratorStore.RegisterUserAsync(model.Id, userName);
                        break;
                    case UserProcessingAction.Decline:
                        await _administratorStore.DeclineUserAsync(model.Id, userName);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
