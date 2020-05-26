using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DataBase;

using A100_AspNetCore.Services.API._DBService;

namespace A100_AspNetCore.Services.API.MapEngineGridService {
    public class MapEngineGridService : IMapEngineGridService {
        public Task<int> GetDefectPagesCount(int ResoultID)
        {
            return Task.Run(() => 0);
        }

        // select * from GetPagedDefects(5020, 20);
        public async Task<List<v_GetVikByUnit>> GetDefectPage(int ResoultID, int Page)
        {
            var result = await MyDB.db.Set<v_GetVik>().FromSql("select * from GetPagedDefects(5020, 20)").ToListAsync();
            // FromSql($"select * from GetPagedDefects({ResoultID}, {Page})");
            return await Task.Run(() => new List<v_GetVikByUnit>());
        }
    }
}