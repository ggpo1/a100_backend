﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API._DBService;
using A100_AspNetCore.Services.Globalsat.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public class GlobalsatService : IGlobalsatService
    {

        #region методы API для WMS

        public async Task<List<DTOWmsGetViks>> GetViksWithAddressation(int ResoultID)
        {
            var viks = await MyDB.db.v_GetVikByUnit.Where(el => el.ResoultID == ResoultID).ToListAsync();
            var wmsAddressation = await MyDB.db.WmsAddressing.Where(el => el.ResoultID == ResoultID).ToListAsync();
            var units = await GetUnitsByResoult(ResoultID);

            List<DTOWmsGetViks> result = new List<DTOWmsGetViks>();

            for (int i = 0; i < viks.Count; i++)
            {
                WmsAddressing addressation = new WmsAddressing();
                try
                {
                    addressation = wmsAddressation.First(el => el.A100Row == viks[i].Row && el.MapUnit == viks[i].UnitName);
                }
                catch (InvalidOperationException)
                {
                    addressation = null;
                }

                var element = await GetElementByID(viks[i].ElementID);
                DTOWmsGetViks validVik = new DTOWmsGetViks(
                    viks[i].VikID,
                    viks[i].Row,
                    viks[i].Frame,
                    viks[i].nLevel,
                    GetRiskLevelByID(viks[i].RiskLevelID),
                    element.ElementName,
                    viks[i].cComment,
                    viks[i].ElementOrientation,
                    viks[i].ManufacturedStillage,
                    viks[i].UnitName,
                    viks[i].ResoultID
                );
                result.Add(validVik);

                if (addressation != null)
                    result[result.Count - 1].Row = addressation.WmsRow;
            }

            return await Task.Run(() => result);
        }

        public async Task<List<DTOWmsGetViks>> GetViksWithAddressationByUnit(int ResoultID, string UnitName)
        {
            var viks = await MyDB.db.v_GetVikByUnit.Where(el => el.ResoultID == ResoultID && el.UnitName == UnitName).ToListAsync();
            var wmsAddressation = await MyDB.db.WmsAddressing.Where(el => el.ResoultID == ResoultID && el.MapUnit == UnitName).ToListAsync();

            List<DTOWmsGetViks> result = new List<DTOWmsGetViks>();
            for (int i = 0; i < viks.Count; i++)
            { 
                WmsAddressing wmsAddress = new WmsAddressing();
                try
                {
                    wmsAddress = wmsAddressation.First(adr => adr.A100Row == viks[i].Row);
                }
                catch (InvalidOperationException)
                {
                    wmsAddress = null;
                }

                var element = await GetElementByID(viks[i].ElementID);
                DTOWmsGetViks validVik = new DTOWmsGetViks(
                    viks[i].VikID,
                    viks[i].Row,
                    viks[i].Frame,
                    viks[i].nLevel,
                    GetRiskLevelByID(viks[i].RiskLevelID),
                    element.ElementName,
                    viks[i].cComment,
                    viks[i].ElementOrientation,
                    viks[i].ManufacturedStillage,
                    viks[i].UnitName,
                    viks[i].ResoultID
                );
                result.Add(validVik);

                if (wmsAddress != null)
                    result[result.Count - 1].Row = wmsAddress.WmsRow;
            };

            return await Task.Run(() => result);
        }

        // Метод получения данных об ударах с дополнительными полями
        public async Task<List<Dictionary<string, object>>> GetBangsWithWmsData(int ResoultID)
        {
            List<v_GetBang> bangs = await MyDB.db.v_GetBang.Where(el => el.ResoultID == ResoultID).ToListAsync();
            List<v_GetWmsFields> wmsFields = await MyDB.db.v_GetWmsFields.Where(el => el.ResoultID == ResoultID).ToListAsync();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var bang in bangs)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                foreach (var property in bang.GetType().GetProperties())
                {
                    string fieldName = property.Name;
                    string _normal = "";
                    if (fieldName.Length == 2) _normal = fieldName.ToLower();
                    else _normal = fieldName[0].ToString().ToLower() + fieldName.Substring(1);
                    temp.Add(_normal, property.GetValue(bang));
                }

                var fields = wmsFields.Where(el => el.SensorID == bang.SensorID).ToList();

                foreach (var field in fields)
                    temp.Add(field.FieldName, field.Value);

                result.Add(temp);
            }

            return await Task.Run(() => result);
        }

        // Метод получения углов с датчиков с дополнительными полями
        public async Task<List<Dictionary<string, object>>> GetDeviationsWithWmsData(int ResoultID)
        {
            List<v_GetGlobalsatDeviation> deviations = await MyDB.db.v_GetGlobalsatDeviation.Where(el => el.ResoultID == ResoultID).ToListAsync();
            List<v_GetWmsFields> wmsFields = await MyDB.db.v_GetWmsFields.Where(el => el.ResoultID == ResoultID).ToListAsync();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var deviation in deviations)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();

                foreach (var property in deviation.GetType().GetProperties())
                {
                    string fieldName = property.Name;
                    string _normal = fieldName[0].ToString().ToLower() + fieldName.Substring(1);
                    temp.Add(_normal, property.GetValue(deviation));
                }

                var fields = wmsFields.Where(el => el.SensorID == deviation.SensorID).ToList();
                foreach (var field in fields)
                    temp.Add(field.FieldName, field.Value);

                result.Add(temp);
            }


            return await Task.Run(() => result);
        }

        // Метод для получения списка повреждений
        public Task<List<v_GetVik>> GetVik(int ResoultID)
        {
            return MyDB.db.v_GetVik.Where(el => el.ResoultID == ResoultID && el.ShowMode == 2).ToListAsync();
        }

        // Метод для получения фотографии к повреждению
        public async Task<DTOPhoto> GetVikPhoto(int ResoultID, int VikID)
        {
            string appData = @"C:/inetpub/wwwroot/asti/Content";
            string photoPath = $"{appData}/{ResoultID}/VIK/{VikID}.jpg";

            if (File.Exists(photoPath))
            {
                FileStream objfilestream = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
                int len = (int)objfilestream.Length;
                Byte[] bImg = new Byte[len];
                objfilestream.Read(bImg, 0, len);
                objfilestream.Close();
                DTOPhoto resp = new DTOPhoto { photo = bImg };
                return await Task.Run(() => resp);
            }
            DTOPhoto nullResp = new DTOPhoto { photo = null };
            return await Task.Run(() => nullResp);
        }

        #endregion


        #region методы для дашборда

        public async Task<object> SetAddressationRows(List<WmsAddressing> Rows)
        {
            foreach (var row in Rows)
            {
                if (row.ID <= -1)
                {
                    MyDB.db.WmsAddressing.Add(new WmsAddressing() 
                    {
                        A100Row=row.A100Row,
                        WmsRow=row.WmsRow,
                        ResoultID=row.ResoultID,
                        MapUnit=row.MapUnit
                    });
                }
                else
                {
                    var _row = MyDB.db.WmsAddressing.FirstOrDefault(el => el.ID == row.ID);
                    _row.WmsRow = row.WmsRow;
                    MyDB.db.Update(_row);
                }
            }

            MyDB.db.SaveChanges();

            return await Task.Run(() => new { sendObj = Rows });
        }

        public async Task<List<WmsAddressing>> GetWmsAddressing(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.WmsAddressing.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }

        public async Task<List<v_GetUniqRows>> GetStillagesRowsByResoultID(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.v_GetUniqRows.Where(el => el.ResoultID == ResoultID && el.Row.Length != 0).ToListAsync());
        }

        // Метод для получения списка блоков, которые есть в проекте
        public async Task<List<string>> GetUnitsByResoult(int ResoultID)
        {
            var dbUnits = await MyDB.db.v_GetUnits.Where(el => el.ResoultID == ResoultID).ToListAsync();

            List<string> unitsList = new List<string>();
            foreach (var unit in dbUnits)
                unitsList.Add(unit.UnitName);

            return await Task.Run(() => unitsList);
        }

        // Метод для получения всех дополнительных полей, которые есть в проекте
        public async Task<List<v_GetWmsFields>> GetWmsFields(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.v_GetWmsFields.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }

        // Метод для получения всех датчиков, закрепленных к проекту
        public async Task<List<GlobalsatSensors>> GetSensorsByResoult(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.GlobalsatSensors.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }

        // Метод добавления дополнительного поля WMS
        public async Task<WmsFields> AddNewWmsField(DTOAddWmsField NewField)
        {
            WmsFields _temp = new WmsFields()
            {
                SensorID = NewField.SensorID,
                FieldName = NewField.FieldName,
                Value = NewField.Value,
                ResoultID = NewField.ResoultID
            };
            var result = MyDB.db.WmsFields.Add(_temp);
            MyDB.db.SaveChanges();
            return await Task.Run(() => result.Entity);
        }

        // Метод удаления дополнительного поля WMS
        public async Task<object> RemoveWmsField(int ID)
        {
            var field = await MyDB.db.WmsFields.FirstOrDefaultAsync(el => el.ID == ID);
            var result = MyDB.db.WmsFields.Remove(field);
            MyDB.db.SaveChanges();
            return await Task.Run(() => result.Entity);
        }

        // Метод для получения списка всех сенсоров в системе
        public async Task<List<GlobalsatSensors>> GetSensors()
        {
            return await Task.Run(() => MyDB.db.GlobalsatSensors.ToListAsync());
        }

        #endregion


        #region методы для Globalsat

        // Метод для получения данных об ударе от Globalsat
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
                response.StatusCode = 500;
                return await Task.Run(() => response);
            }
        }

        // Метод для получения данных об отклонениях от Globalsat
        public async Task<Object> AddDeviations(List<AddDeviationsRequest> data)
        {
            int i = 0;
            dynamic response = new ExpandoObject();
            try
            {
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
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.StatusCode = 500;
                return await Task.Run(() => response);
            }
        }

        #endregion


        #region остальные методы

        public string GetRiskLevelByID(int? ID)
        {
            if (ID == 1)
                return "GREEN";
            else if (ID == 2)
                return "YELLOW";
            else if (ID == 3)
                return "RED";
            else
                return "";
        }

        public async Task<Element> GetElementByID(int? ID)
        {
            return await MyDB.db.Element.FirstOrDefaultAsync(el => el.ElementId == ID);
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

        #endregion

    }
}