using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API.MapEngineGridService;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Models.A100_Models.DataBase;

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
        [Route("back/defects")]
        public async Task<List<Dictionary<string, object>>> GetSeparatedDefects(int ResoultID, int Page)
        {
            return await service.GetDefectPage(ResoultID, Page);
        }

        [HttpGet]
        [Route("defects")]
        public async Task<List<Dictionary<string, object>>> GetDefectsPageSepByBack(int ResoultID, int Page)
        {
            return await service.GetDefectPageSepByBack(ResoultID, Page);
        }

        [HttpGet]
        [Route("all/defects")]
        public async Task<List<v_GetVikByUnit>> GetWholeDefects(int ResoultID)
        {
            return await service.GetWholeDefects(ResoultID);
        }

        [HttpGet]
        [Route("headers/defects")]
        public async Task<List<HeaderItem>> GetDefectsHeaders()
        {
            return await service.GetDefectsHeaders();
        }

        [HttpGet]
        [Route("additional/elements")]
        public async Task<List<Element>> GetElements()
        {
            return await service.GetElements();
        }

        [HttpGet]
        [Route("additional/defecttypes")]
        public async Task<List<DefectType>> GetDefectTypes()
        {
            return await service.GetDefectTypes();
        }

        #endregion
    }
}