using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API._DBService;
using A100_AspNetCore.Services.Globalsat.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public class GlobalsatService : IGlobalsatService
    {

        public async Task<List<string>> GetUnitsByResoult(int ResoultID)
        {
            var dbUnits = await MyDB.db.v_GetUnits.Where(el => el.ResoultID == ResoultID).ToListAsync();

            List<string> unitsList = new List<string>();
            foreach (var unit in dbUnits)
                unitsList.Add(unit.UnitName);

            return await Task.Run(() => unitsList);
        }

        public async Task<List<v_GetWmsFields>> GetWmsFields(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.v_GetWmsFields.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }





        public async Task<List<GlobalsatSensors>> GetSensors()
        {
            return await Task.Run(() => MyDB.db.GlobalsatSensors.ToListAsync());
        }

        public async Task<List<GlobalsatSensors>> GetSensorsByResoult(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.GlobalsatSensors.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }

        public async Task<Object> AddBang(List<AddBangRequest> data)
        {
            int i = 0;
            dynamic response = new ExpandoObject();
            try
            {
                foreach (var el in data)
                {
                    var sensor = await MyDB.db.v_GetSensor.FirstOrDefaultAsync(sEl => sEl.SensorID == el.SensorId);
                    await MyDB.db.GlobalsatBangs.AddAsync(new GlobalsatBangs()
                    {
                        SensorID = el.SensorId,
                        Status = "",
                        Strength = el.Strength,
                        BangDate = el.BangDate,
                        ResoultID = sensor.ResoultID
                    });
                    i++;
                }
                await MyDB.db.SaveChangesAsync();
                response.Message = i + " added";
                response.StatusCode = 200;
                return await Task.Run(() => response);
            }
            catch (Exception e)
            {
                response.Message = "sensor with that id does not exist!";
                response.StatusCode = 400;
                return await Task.Run(() => response);
            }
        }

        public async Task<Object> AddDeviations(List<AddDeviationsRequest> data)
        {
            int i = 0;
            dynamic response = new ExpandoObject();
            //try
           // {
                foreach (var el in data)
                {
                    var sensor = await MyDB.db.v_GetSensor.FirstOrDefaultAsync(sEl => sEl.SensorID == el.SensorId);
                    // var deviations = await MyDB.db.GlobalsatDeviations.Where(dEl => dEl.SensorID == sensor.SensorID).ToListAsync();
                    // MyDB.db.GlobalsatDeviations.RemoveRange(deviations);
                    // MyDB.db.Entry(deviations[0]).State = EntityState.Deleted;
                    if (el.DeviationValue != null)
                    {
                        await MyDB.db.GlobalsatDeviations.AddAsync(new GlobalsatDeviations()
                        {
                            SensorID = el.SensorId,
                            DeviationValue = el.DeviationValue,
                            DeviationDate = el.DeviationDate,
                            ResoultID = sensor.ResoultID
                        });
                        i++;
                    }
                }
                await MyDB.db.SaveChangesAsync();
                response.Message = i + " added";
                response.StatusCode = 200;
                return await Task.Run(() => response);
            //}
            //catch (Exception e)
            //{
                //response.Message = e.Message;
                //response.StatusCode = 400;
              //  return await Task.Run(() => response);
            //}
        }

        public async Task<List<v_GetBang>> GetBangsByResoult(int resoultID)
        {
            return await MyDB.db.v_GetBang.Where(el => el.ResoultID == resoultID && el.Strength != null).OrderByDescending(el => el.BangDate).ToListAsync();
        }

        public async Task<List<v_GetGlobalsatDeviation>> GetDeviationsByResoult(int resoultID)
        {
            var deviations = await MyDB.db.v_GetGlobalsatDeviation.Where(el => el.ResoultID == resoultID && el.DeviationValue != null).ToListAsync();
            List<v_GetGlobalsatDeviation> lastDeviations = new List<v_GetGlobalsatDeviation>();

            foreach (var el in deviations)
            {
                v_GetGlobalsatDeviation last = el;                
                var sensorDeviations = deviations.Where(_el => el.SensorID == _el.SensorID).ToList();
                foreach (var _el in sensorDeviations)
                {
                    if (last.DeviationDate.CompareTo(_el.DeviationDate) <= 0) last = _el;
                }
                if (last.SensorID != null)
                    lastDeviations.Add(last);
            }
            
            
            
            /*
            foreach (var el in deviations)
            {
                var all = deviations.Where(_el => _el.SensorID == el.SensorID).ToList();
                v_GetGlobalsatDeviation last = new v_GetGlobalsatDeviation();
                foreach (var _el in all)
                {
                    if (el.DeviationDate.CompareTo(_el.DeviationDate) < 0) last = _el;
                }
                if (last.SensorID != null)
                    lastDeviations.Add(last);
            }
            */
            return await Task.Run(() => lastDeviations);
        }
    }
}