using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;

namespace A100_AspNetCore.Services.API.MapEngineGridService {
    public interface IMapEngineGridService {
        Task<int> GetDefectPagesCount(int ResoultID); // params
        Task<Dictionary<string, bool>> IsDefectPageValid(int ResoultID, int Page);
        Task<List<Dictionary<string, HeaderItem>>> GetDefectsHeaders();
        Task<List<Dictionary<string, object>>> GetDefectPage(int ResoultID, int Page); // params
    }
}
