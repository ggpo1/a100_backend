using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.MapEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.MapService
{
    public interface IMapService
    {
        Task<List<Map>> GetMap(int ResoultID);
        Task<List<v_GetVik>> GetDefect(int ResoultID, string UnitName, List<int> StillagesID);
        Task<List<string>> GetUnits(int ResoultID);
    }
}
