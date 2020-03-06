using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Services.API._DBService;
using A100_AspNetCore.Services.Globalsat.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public class GlobalsatService : IGlobalsatService
    {
        public async Task<Object> AddBang(AddBangRequest data)
        {
            // Таблица GlobalsatBangs
            List<GlobalsatSensors> sensors = await MyDB.db.GlobalsatSensors.ToListAsync();
            
            
            
            dynamic response = new ExpandoObject();
            response.message = "not implemented yet";
            return await Task.Run(() => response);
        }

        public async Task<Object> AddDeviations(List<AddDeviationsRequest> data)
        {
            dynamic response = new ExpandoObject();;
            response.message = "not implemented yet";
            return await Task.Run(() => response);
        }
    }
}