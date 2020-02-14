﻿using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API.SpecificationsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.API.Controllers
{

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SpecificationsController : ControllerBase
    {
        ISpecificationsService service;

        public SpecificationsController(ISpecificationsService service)
        {
            this.service = service;
        }


        #region Методы

        // GET - метод, который получает названия и типы уровней риска
        [HttpGet]
        [Route("DictionaryRiskLevel")]
        public async Task<List<RiskLevel>> DictionaryRiskLevel()
        {
            return await service.DictionaryRiskLevel();
        }

        // GET - метод, который получает типы дефектов
        [HttpGet]
        [Route("GetDefectTypes")]
        public async Task<List<DTODefectType>> GetDefectTypes()
        {
            return await service.GetDefectTypes();
        }

        // GET - метод, который получает спецификации (типы стелажей) по ResoultID
        [HttpPost]
        [Route("GetDeviation")]
        public async Task<List<Deviation>> GetDeviation([FromBody]Deviation dev)
        {
            return await service.GetDeviation(dev.ResoultId);
        }

        // GET - метод, который получает элементы стеллажей по ResoultID
        [HttpPost]
        [Route("GetSpecificationElement")]
        public async Task<List<v_GetSpecificationsElement>> GetSpecificationElement([FromBody]v_GetSpecificationsElement elem)
        {
            return await service.GetSpecificationElement(elem.ResoultID);
        }

        // GET - метод, который получает спецификации (типы стелажей) по ResoultID
        [HttpPost]
        [Route("GetSpecifications")]
        public async Task<List<Specifications>> GetSpecifications([FromBody]Specifications spec)
        {
            return await service.GetSpecifications(spec.ResoultId);
        }

        // GET - метод, который получает названия и типы уровней риска
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<List<ClientPermissions>> GetUserRoles()
        {
            return await service.GetUserRoles();
        }

        [HttpGet]
        [Route("GetElement")]
        public async Task<List<Element>> GetElement()
        {
            return await service.GetElement();
        }

        [HttpGet]
        [Route("GetSpecificationsElementSizes")]
        public async Task<List<v_GetElementSize>> GetElementSizes(int specificationsElementID)
        {
            return await service.GetSpecificationsElementSizes(specificationsElementID);
        }

        [HttpGet]
        [Route("GetSpecificationsSize")]
        public async Task<List<DTOSpecificationsSize>> GetSpecificationsSize(int SpecificationsID)
        {
            return await service.GetSpecificationsWithSizes(SpecificationsID);
        }


        #endregion

    }
}
