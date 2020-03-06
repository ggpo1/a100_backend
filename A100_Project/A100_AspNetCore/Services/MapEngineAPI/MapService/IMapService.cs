using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.MapEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Services.MapEngineAPI.Models.DTO;

namespace A100_AspNetCore.Services.MapEngineAPI.MapService
{
    public interface IMapService
    {
        Task<List<GetUnitsDTO>> GetUnitNamesByResoult(int ResoultID);
        Task<List<Map>> GetMap(int ResoultID);

        Task<List<MapLayer>> GetUnitLayersByUnitName(int ResoultID, string UnitName, string UnitKey);
        // Task<List<>>
        Task<List<v_GetVik>> GetDefect(int ResoultID, string UnitName, List<int> StillagesID);
        Task<List<string>> GetUnits(int ResoultID);
    }
}
