using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DataBase;

using A100_AspNetCore.Services.API._DBService;
using System.Data.SqlClient;
using System;
using A100_AspNetCore.Models.A100_Models.DTO;
using System.Linq;
using System.Dynamic;

namespace A100_AspNetCore.Services.API.MapEngineGridService {
    public class MapEngineGridService : IMapEngineGridService {

        public string ConnectionString = "Server=ASTI\\ASTISQL;Database=ASTI;Trusted_Connection=False;MultipleActiveResultSets=True; User ID=sa;Password=gbhjcKJK4509";
        
        public async Task<int> GetDefectPagesCount(int ResoultID)
        {
            return await Task.Run(() => 0);
        }

        public async Task<List<v_GetVikByUnit>> GetWholeDefects(int ResoultID)
        {
            return await Task.Run(() => MyDB.db.v_GetVikByUnit.Where(el => el.ResoultID == ResoultID).ToListAsync());
        }

        public async Task<List<Dictionary<string, object>>> GetDefectPageSepByBack(int ResoultID, int Page)
        {
            const int PAGE_DEFECTS_COUNT = 10;
            int from = Page * PAGE_DEFECTS_COUNT;
            int to = from + PAGE_DEFECTS_COUNT;

            var defects = await MyDB.db.v_GetVikByUnit.Where(el => el.ResoultID == ResoultID).ToListAsync();

            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
            
            try
            {
                for (int i = from; i < to; i++)
                {
                    Dictionary<string, object> temp = new Dictionary<string, object>();
                    // var expando = new ExpandoObject();
                    // var dictionary = (IDictionary<string, object>)expando;

                    foreach (var property in defects[i].GetType().GetProperties())
                    {
                        string fieldName = property.Name;
                        string _normal = fieldName[0].ToString().ToLower() + fieldName.Substring(1);

                        temp.Add(_normal, property.GetValue(defects[i]));
                    }

                    if (defects[i].RiskLevelID == 1)
                    {
                        temp.Add("backColor", "#88ee9b");
                    }
                    else if (defects[i].RiskLevelID == 2)
                    {
                        temp.Add("backColor", "#fffad2");
                    }
                    else if (defects[i].RiskLevelID == 3)
                    {
                        temp.Add("backColor", "#f37f82");
                    }

                resultList.Add(temp);
                }
            }
            catch (Exception ex) { }

            return await Task.Run(() => resultList);
        }

        public async Task<List<Dictionary<string, object>>> GetDefectPage(int ResoultID, int Page)
        {
            List<Dictionary<string, object>> defects = new List<Dictionary<string, object>>();
            string queryString = "select * from GetPagedDefects(@resoultID, @page)";
            
            using (SqlConnection connection = 
                new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@resoultID", ResoultID));
                command.Parameters.Add(new SqlParameter("@page", Page));
                byte[] freeByteArray = new byte[0];
                
                connection.Open();
                using (SqlDataReader rd = command.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        Dictionary<string, object> readerObject = new Dictionary<string, object>();
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            string fieldName = rd.GetName(i);
                            string _normal = fieldName[0].ToString().ToLower() + fieldName.Substring(1);
                            readerObject.Add(_normal, rd[fieldName]);
                        }

                        if ((int)rd["RiskLevelID"] == 1)
                        {
                            readerObject.Add("backColor", "#88ee9b");
                        }
                        else if ((int)rd["RiskLevelID"] == 2)
                        {
                            readerObject.Add("backColor", "#fffad2");
                        }
                        else if ((int)rd["RiskLevelID"] == 3)
                        {
                            readerObject.Add("backColor", "#f37f82");
                        }
                        defects.Add(readerObject);
                    }
                }
            }
            return await Task.Run(() => defects);
        }

        public async Task<List<Element>> GetElements()
        {
            return await Task.Run(() => MyDB.db.Element.ToListAsync());
        }

        public async Task<List<DefectType>> GetDefectTypes()
        {
            return await Task.Run(() => MyDB.db.DefectType.ToListAsync());
        }

        public Task<Dictionary<string, bool>> IsDefectPageValid(int ResoultID, int Page)
        {
            throw new NotImplementedException();
        }

        

        public async Task<List<HeaderItem>> GetDefectsHeaders()
        {
            List<HeaderItem> defectsHeaders = new List<HeaderItem>();
            defectsHeaders.Add(new HeaderItem()
            {
                Key="row",
                Type="string",
                Title="Ряд",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="frame",
                Type="string",
                Title="Место",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="nLevel",
                Type="string",
                Title="Уровень",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="elementID",
                Type="string",
                Title="Элемент",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="elementSize",
                Type="string",
                Title="Размер",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="defectID",
                Type="string",
                Title="Тип дефекта",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key= "riskLevelID",
                Type="string",
                Title="Уровень риска",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key= "cComment",
                Type="string",
                Title="Комментарий",
                IsHide=true
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key= "updateTime",
                Type="dateTime",
                Title="Дата обнаружения",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="isDone",
                Type="boolean",
                Title="Исправлен",
                IsHide=false
            });

            return await Task.Run(() => defectsHeaders); 
        }

        
    }
}