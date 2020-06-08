using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.Globalsat.Models.DTO;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public interface IGlobalsatService
    {
        // Task<Object> AddSensors();
        // Task<Object> AddRacks();
        Task<Object> AddBang(List<AddBangRequest> data);
        Task<List<GlobalsatSensors>> GetSensors();
        Task<List<GlobalsatSensors>> GetSensorsByResoult(int ResoultID);
        Task<List<v_GetBang>> GetBangsByResoult(int resoultID);
        
        Task<Object> AddDeviations(List<AddDeviationsRequest> data);
        Task<List<v_GetGlobalsatDeviation>> GetDeviationsByResoult(int resoultID);

        Task<List<string>> GetUnitsByResoult(int ResoultID);
        Task<List<v_GetWmsFields>> GetWmsFields(int ResoultID);
        Task<List<Dictionary<string, object>>> GetBangsWithWmsData(int ResoultID);
        Task<List<Dictionary<string, object>>> GetDeviationsWithWmsData(int ResoultID);
        Task<WmsFields> AddNewWmsField(DTOAddWmsField NewField);
        Task<object> RemoveWmsField(int ID);
    }
}