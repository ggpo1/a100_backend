using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.Globalsat.GlobalsatService;
using A100_AspNetCore.Services.Globalsat.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class GlobalsatController : Controller
    {
        IGlobalsatService service;

        public GlobalsatController(IGlobalsatService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("sensors")]
        public async Task<List<GlobalsatSensors>> GetSensorsByResoultID(int ResoultID)
        {
            return await service.GetSensorsByResoult(ResoultID);
        }

        [HttpGet]
        [Route("sensors/all")]
        public async Task<List<GlobalsatSensors>> GetSensors()
        {
            return await service.GetSensors();
        }

        [HttpGet]
        [Route("bangs")]
        public async Task<List<v_GetBang>> GetBangsByResoult(int resoultID)
        {
            return await service.GetBangsByResoult(resoultID);
        }
        
        [HttpPost]
        [Route("bangs")]
        public async Task<Object> AddBang([FromBody] List<AddBangRequest> data)
        {
            var response = (dynamic) await service.AddBang(data);
            if (response.StatusCode == 200)
                return Ok(response);
            else if (response.StatusCode == 400)
                return BadRequest(response);
            return response;
        }


        [HttpGet]
        [Route("corners")]
        public async Task<List<v_GetGlobalsatDeviation>> GetCornersByResoult(int resoultID)
        {
            return await service.GetDeviationsByResoult(resoultID);
        }

        [HttpPost]
        [Route("corners")]
        public async Task<Object> AddDeviations([FromBody] List<AddDeviationsRequest> data)
        {
            var response = (dynamic) await service.AddDeviations(data);
            if (response.StatusCode == 200)
                return Ok(response);
            else if (response.StatusCode == 400)
                return BadRequest(response);
            return response;
        }
    }
}