using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.Globalsat.Models.DTO;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public interface IGlobalsatService
    {
        // Task<Object> AddSensors();
        // Task<Object> AddRacks();
        Task<Object> AddBang(List<AddBangRequest> data);
        Task<List<v_GetBang>> GetBangsByResoult(int resoultID);
        
        Task<Object> AddDeviations(List<AddDeviationsRequest> data);
        Task<List<v_GetGlobalsatDeviation>> GetDeviationsByResoult(int resoultID);
        
    }
}