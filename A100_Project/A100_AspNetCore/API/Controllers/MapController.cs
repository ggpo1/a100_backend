using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.MapEngineAPI.MapService;
using A100_AspNetCore.Services.MapEngineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Services.MapEngineAPI.Models.DTO;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/mapengine/[Controller]")]
    [ApiController]
    public class MapController
    {
        IMapService service;

        public MapController(IMapService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<Map>> GetMap(int ResoultID)
        {
            return await service.GetMap(ResoultID);
        }

        [HttpGet]
        [Route("unitnames")]
        public async Task<List<GetUnitsDTO>> GetUnitsByResoult(int ResoultID)
        {
            return await service.GetUnitNamesByResoult(ResoultID);
        }

        [HttpGet]
        [Route("byparams")]
        public async Task<List<MapLayer>> GetMapByParams(int ResoultID, string UnitName, string UnitKey)
        {
            return await service.GetUnitLayersByUnitName(ResoultID, UnitName, UnitKey);
        }

    }
}
