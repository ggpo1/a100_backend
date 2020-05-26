using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API.MapEngineGridService;


namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SeparatedDataController : ControllerBase
    {
        IMapEngineGridService service;

        public SeparatedDataController(IMapEngineGridService service)
        {
            this.service = service;
        }

        #region Defects queries section
    
        [HttpGet]
        [Route("defects")]
        public async Task<List<v_GetVikByUnit>> GetSeparatedDefects(int ResoultID, int Page)
        {
            return await service.GetDefectPage(ResoultID, Page);
        }

        #endregion
    }
}