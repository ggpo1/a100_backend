using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API.VikService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A100_AspNetCore.API.Controllers
{

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class VIKController : ControllerBase
    {
        IVikService service;

        public VIKController(IVikService service)
        {
            this.service = service;
        }


        #region Методы контроллера


        // GET - метод, который получает VIK по ResoultID



        [HttpPost]
        [Route("GetVik")]
        public async Task<List<v_GetVik>> GetVik([FromBody]v_GetVik vik)
        {
            return await service.GetVik(vik.ResoultID);
        }

        [HttpPost]
        [Route("Viks")]
        public async Task<DTOAddVik> AddVik([FromBody] Vik vik)
        {
            return await service.AddNewVik(vik);
        }

        // GET - метод, который получает названия VIK
        [HttpGet]
        [Route("GetVikNames")]
        public async Task<List<VikElement>> GetVikNames()
        {
            return await service.GetVikNames();
        }


        [HttpGet]
        [Route("GetViksByUnit")]
        public async Task<List<DTOVikByUnit>> GetViksByUnit(int resoultID,string unitName)
        {
            return await service.GetViksByUnit(resoultID, unitName);
        }

        [HttpGet]
        [Route("GetCameraEvents")]
        public async Task<CameraEvents> GetCameraEvents(string userName)
        {
            return await service.GetCameraEvents(userName);
        }
        [HttpDelete]
        [Route("GetCameraEvents")]
        public async Task<string> DeleteCameraEvents(string username)
        {
            return await service.DeleteCameraEvents(username);
        }
        [HttpPost]
        [Route("GetCameraEvents")]
        public async Task<string> RegisterCameraEvent([FromBody]CameraEvents cameraEvent)
        {
            return await service.RegisterCameraEvent(cameraEvent);
        }


        [HttpGet]
        [Route("GetVikPhoto")]
        public async Task<object> GetVikPhoto(int resoultID, int vikID)
        {
            return await service.GetVikPhoto(resoultID, vikID);
        }

        [HttpPost]
        [Route("UpdateCameraEvent")]
        public async Task<CameraEvents> UpdateCameraEvent([FromBody] CameraEvents cEvnt)
        {
            return await service.UpdateCameraEvent(cEvnt);
        }

        [HttpGet]
        [Route("GetViksByPartialTOID")]
        public async Task<List<DTOVikByUnit>> GetViksByPartialTOID(int partialTOID)
        {
            return await service.GetViksByPartialTO(partialTOID);
        }

        [HttpPost]
        [Route("AddVikPhoto")]
        public async Task<object> AddVikPhoto([FromBody] DTOAddVikPhoto dtoPhoto)
        {
            return await service.AddVikPhoto(dtoPhoto.ResoultID, dtoPhoto.VikID, dtoPhoto.Photo);
        }

        [HttpPost]
        [Route("UpdateVik")]
        public async Task<object> UpdateVik(int VikID, [FromBody]Vik vik)
        {
            return await service.UpdateVik(VikID, vik);
        }
        #endregion


    }
}
