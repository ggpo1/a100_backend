using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.Globalsat.GlobalsatService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class WmsController : Controller
    {
        IGlobalsatService service;

        public WmsController(IGlobalsatService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("units")]
        public async Task<List<string>> GetUnits(int ResoultID)
        {
            return await service.GetUnitsByResoult(ResoultID);
        }

        [HttpGet]
        [Route("fields")]
        public async Task<List<v_GetWmsFields>> GetWmsFields(int ResoultID)
        {
            return await service.GetWmsFields(ResoultID);
        }

        [HttpGet]
        [Route("bangs")]
        public async Task<List<Dictionary<string, object>>> GetBangsWithWmsFields(int ResoultID)
        {
            return await service.GetBangsWithWmsData(ResoultID);
        }

        [HttpGet]
        [Route("deviations")]
        public async Task<List<Dictionary<string, object>>> GetDeviationsWithWmsFields(int ResoultID)
        {
            return await service.GetDeviationsWithWmsData(ResoultID);
        }

        [HttpPost]
        [Route("fields")]
        public async Task<WmsFields> AddNewWmsField([FromBody] DTOAddWmsField NewField)
        {
            return await service.AddNewWmsField(NewField);
        }

        [HttpDelete]
        [Route("fields")]
        public async Task<object> RemoveWmsField(int ID)
        {
            return await service.RemoveWmsField(ID);
        }

        [HttpGet]
        [Route("sysrows")]
        public async Task<List<v_GetUniqRows>> GetUniqRowsByResoult(int ResoultID)
        {
            return await service.GetStillagesRowsByResoultID(ResoultID);
        }

        [HttpGet]
        [Route("wmsrows")]
        public async Task<List<WmsAddressing>> GetWmsAddressing(int ResoultID)
        {
            return await service.GetWmsAddressing(ResoultID);
        }

        [HttpPost]
        [Route("wmsrows")]
        public async Task<object> SetAddressingRows([FromBody] List<WmsAddressing> Rows)
        {
            return await service.SetAddressationRows(Rows);
        }

    }
}
