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
                        defects.Add(readerObject);
                    }
                }
            }
            return await Task.Run(() => defects);
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
                Key="place",
                Type="string",
                Title="Место",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="level",
                Type="string",
                Title="Уровень",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="elementName",
                Type="string",
                Title="Элемент",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="size",
                Type="string",
                Title="Размер",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="defectType",
                Type="string",
                Title="Тип дефекта",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="riskLevel",
                Type="string",
                Title="Уровень риска",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="comment",
                Type="string",
                Title="Комментарий",
                IsHide=true
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="browseDate",
                Type="string",
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