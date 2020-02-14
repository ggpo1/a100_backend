using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API._DBService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.API.PartialService
{
    public class PartialService : IPartialService
    {
        public async Task<object> StopPartialTO(int partialTOID)
        {
            try
            {
                var toUpdate = MyDB.db.PartialTo.FirstOrDefault(elem => elem.PartialToid == partialTOID);
                toUpdate.IsOver = true;
                MyDB.db.PartialTo.Update(toUpdate);
                MyDB.db.SaveChanges();
                var check = MyDB.db.PartialTo.FirstOrDefault(el => el.PartialToid == partialTOID);
                if (check.IsOver == false)
                    return await Task.Run(() => (object)new { status = "no" });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => (object)new { status = ex.Message });
            }
            return await Task.Run(() => (object)new { status = "ok" });
        }
        public async Task<object> AddNewPartialProgress(List<PartialTOProgress> progress)
        {
            try
            {
                foreach (var el in progress)
                    MyDB.db.PartialTOProgress.Add(el);
                MyDB.db.SaveChanges();
            }
            catch (Exception ex)
            {
                return await Task.Run(() => (object)new { status = ex.Message });
            }
            return Task.Run(() => (object)new { status = "ok" });
        }

        public async Task<PartialTo> AddNewPartialTo(PartialTo partialTo)
        {
            MyDB.db.PartialTo.Add(partialTo);
            MyDB.db.SaveChanges();
            return await Task.Run(() => partialTo);
        }

        public Task<List<PartialTo>> GetPartialTo()
        {
            return Task.Run(() => MyDB.db.PartialTo.ToList());
        }

        public async Task<List<DTOPartial>> GetPartialToByResoultID(int ResoultID)
        {
            List<PartialTo> pList = MyDB.db.PartialTo.Where(elem => elem.ResoultId == ResoultID).ToList();
            List<DTOPartial> dPList = new List<DTOPartial>();
            foreach (var el in pList)
            {
                var partialElems = MyDB.db.PartialTOProgress.Where(elem => elem.PartialTOID == el.PartialToid).ToList();
                var partial = new DTOPartial(el.PartialToid, el.PartialTodate,
                    el.WarhouseId, el.EmployeeName,
                    el.TransformRow, el.UpdateTime,
                    el.ResoultId, el.IsOver);
                partial.PartialTOElements = partialElems;
                dPList.Add(partial);
               
            }
            return await Task.Run(() => dPList);
        }
    }
}
