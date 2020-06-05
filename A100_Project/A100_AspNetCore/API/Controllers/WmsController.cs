using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.Globalsat.GlobalsatService;
using Microsoft.AspNetCore.Mvc;
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

    }
}
