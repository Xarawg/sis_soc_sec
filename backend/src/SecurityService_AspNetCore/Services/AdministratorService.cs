﻿using AutoMapper;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Models.ControllerDTO.User;
using SecurityService_Core.Models.ControllerDTO.Administrator;
using SecurityService_Core.Models.DTO;
using SecurityService_Core.Models.DB;

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
    }
}
