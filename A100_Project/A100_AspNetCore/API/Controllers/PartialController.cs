using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API.PartialService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PartialController : ControllerBase
    {
        IPartialService service;
        public PartialController(IPartialService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<PartialTo>> GetPartials()
        {
            return await service.GetPartialTo();
        }

        [HttpGet]
        [Route("GetPartialsByResoult")]
        public async Task<List<DTOPartial>> GetPartialsByResoultID(int ResoultID)
        {
            return await service.GetPartialToByResoultID(ResoultID);
        }

        [HttpPost]
        [Route("AddPartial")]
        public async Task<PartialTo> AddNewPartialTo([FromBody]PartialTo PartialTo)
        {
            return await service.AddNewPartialTo(PartialTo);
        }

        [HttpPost]
        [Route("AddNewPartialProgress")]
        public async Task<object> AddNewPartialProgress([FromBody]List<PartialTOProgress> progress)
        {
            return await service.AddNewPartialProgress(progress);
        }
        [HttpPost]
        [Route("StopPartialTO")]
        public async Task<object> StopPartialTO([FromBody]PartialTo partial)
        {
            return await service.StopPartialTO(partial.PartialToid);
        }
    }
}
