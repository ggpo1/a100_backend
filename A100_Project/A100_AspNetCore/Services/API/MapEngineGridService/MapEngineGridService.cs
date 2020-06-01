using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DataBase;

using A100_AspNetCore.Services.API._DBService;
using System.Data.SqlClient;
using System;
using A100_AspNetCore.Models.A100_Models.DTO;

namespace A100_AspNetCore.Services.API.MapEngineGridService {
    public class MapEngineGridService : IMapEngineGridService {

        public string ConnectionString = "Server=ASTI\\ASTISQL;Database=ASTI;Trusted_Connection=False;MultipleActiveResultSets=True; User ID=sa;Password=gbhjcKJK4509";
        
        public async Task<int> GetDefectPagesCount(int ResoultID)
        {
            return await Task.Run(() => 0);
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
                            readerObject.Add(fieldName, rd[fieldName]);
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
                Key="Row",
                Type="string",
                Title="Ряд",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="Frame",
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
                Key="ElementID",
                Type="string",
                Title="Элемент",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="ElementSize",
                Type="string",
                Title="Размер",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="DefectID",
                Type="string",
                Title="Тип дефекта",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key= "RiskLevelID",
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
                Key= "UpdateTime",
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