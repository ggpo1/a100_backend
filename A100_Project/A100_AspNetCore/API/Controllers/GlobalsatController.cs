using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Services.Globalsat.GlobalsatService;
using A100_AspNetCore.Services.Globalsat.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class GlobalsatController
    {
        IGlobalsatService service;

        public GlobalsatController(IGlobalsatService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("bangs")]
        public async Task<Object> AddBang([FromBody] AddBangRequest data)
        {
            return await service.AddBang(data);
        }

        [HttpPost]
        [Route("corners")]
        public async Task<Object> AddDeviations([FromBody] List<AddDeviationsRequest> data)
        {
            return await service.AddDeviations(data);
        }
    }
}