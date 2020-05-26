using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;

namespace A100_AspNetCore.Services.API.MapEngineGridService {
    public interface IMapEngineGridService {
        Task<int> GetDefectPagesCount(int ResoultID); // params
        Task<List<v_GetVikByUnit>> GetDefectPage(int ResoultID, int Page); // params
    }
}
