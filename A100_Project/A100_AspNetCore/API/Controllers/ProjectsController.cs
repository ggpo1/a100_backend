using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API.Projects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        public class Resoult
        {
            public Resoult(int resoultID)
            {
                this.ResoultID = resoultID;
            }
            public int ResoultID { get; set; }
        }

        IProjectsService service;

        public ProjectsController(IProjectsService service)
        {
            this.service = service;
        }


        // Информация о проекте
        [HttpPost]
        [Route("GetWork")]
        public async Task<v_GetWork> GetWork([FromBody]v_GetWork work)
        {
            return await service.GetWork(work.WarhouseID);
        }

        // Список проектов пользователя
        [HttpPost]
        [Route("GetUserProjects")]
        public async Task<List<v_GetProjects>> GetUserProjects([FromBody]v_GetProjects projects)
        {
            return await service.GetUserProjects(projects.UserName);
        }

        // GET - метод, который получает ТЗ
        [HttpPost]
        [Route("GetProjectData")]
        public async Task<Control> GetProjectData([FromBody]Control control)
        {
            return await service.GetProjectData(control.ProjectNumber);
        }

        [HttpGet]
        [Route("GetProjectIDInfo")]
        public async Task<Resoult> GetProjectIDInfo(string projectNumber)
        {
            v_GetControl control = await service.GetProjectIDInfo(projectNumber);
            return new Resoult(control.ResoultID);
        }

        [HttpGet]
        [Route("GetUnitNames")]
        public async Task<List<v_GetUnitNames>> GetUnitNames(string projectNumber)
        {
            return await service.GetUnitNames(projectNumber);
        }
    }
}
