using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.Responsed;
// using A100_AspNetCore.Services.API.BackupService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A100_AspNetCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        // IBackupService service;
        // public BackupController(IBackupService service)
        // {
        //     this.service = service;
        // }

        // [HttpGet]
        // [Route("GetASTIBaks")]
        // public async Task<List<BackUpFile>> GetASTIBaks()
        // {
        //     return await service.GetASTIBaks();
        // }
    }
}