using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DataBase;

using A100_AspNetCore.Services.API._DBService;
using System.Data.SqlClient;
using System;
using LinqToDB.Data;
using Castle.MicroKernel.Registration;
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

        /*
         {
							key: 'place',
							type: 'string',
							title: 'Место',
							isHide: false
						},
						{
							key: 'level',
							type: 'string',
							title: 'Уровень',
							isHide: false
						},
						{
							key: 'elementName',
							type: 'string',
							title: 'Элемент',
							isHide: false
						},
						{
							key: 'size',
							type: 'string',
							title: 'Размер',
							isHide: false
						},
						{
							key: 'defectType',
							type: 'string',
							title: 'Тип дефекта',
							isHide: false
						},
						{
							key: 'riskLevel',
							type: 'string',
							title: 'Уровень риска',
							isHide: false
						},
						{
							key: 'comment',
							type: 'string',
							title: 'Комментарий',
							isHide: true
						},
						{
							key: 'browseDate',
							type: 'string',
							title: 'Дата обнаружения',
							isHide: false
						},
						{
							key: 'isDone',
							type: 'boolean',
							title: 'Исправлен',
							isHide: false
						}
         */

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
                Key="",
                Type="",
                Title="",
                IsHide=false
            });
            defectsHeaders.Add(new HeaderItem()
            {
                Key="isDone",
                Type="boolean",
                Title="Исправлен",
                IsHide=false
            });

            return defectsHeaders; 
        }

        
    }
}