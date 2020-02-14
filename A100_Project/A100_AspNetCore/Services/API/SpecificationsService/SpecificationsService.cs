using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using A100_AspNetCore.Services.API._DBService;
using Microsoft.EntityFrameworkCore;

namespace A100_AspNetCore.Services.API.SpecificationsService
{
    public class SpecificationsService : ISpecificationsService
    {
        // GET - метод, который получает названия и типы уровней риска
        public Task<List<RiskLevel>> DictionaryRiskLevel()
        {
            return MyDB.db.RiskLevel.ToListAsync();
        }

        public Task<List<DTOSpecificationsSize>> GetSpecificationsWithSizes(int SpecificationsID)
        {
            List<v_GetSpecificationsWIthSize> list = MyDB.db.v_GetSpecificationsWIthSize.Where(elem => elem.SpecificationsID == SpecificationsID).ToList();
            List<DTOSpecificationsSize> sizesList = new List<DTOSpecificationsSize>() { };
            List<int> alreadySave = new List<int>(); 
            foreach (var elem in list)
            {
                bool already = false;
                foreach (var size in sizesList)
                {
                    if (size.ElementID == elem.ElementID)
                        already = true;
                }

                if (already)
                {
                    foreach (var size in sizesList)
                    {
                        if (elem.ElementID == size.ElementID)
                        {
                            size.ElementSizes.Add(new ElementSize(elem.SpecificationsElementID, elem.ElementSize));
                        }
                    }
                }
                else
                {
                    sizesList.Add(new DTOSpecificationsSize(
                           elem.ElementID,
                           elem.ElementName,
                           new List<ElementSize>() { new ElementSize(elem.SpecificationsElementID, elem.ElementSize) }
                        ));
                }
            }

            return Task.Run(() => sizesList);
        }

        // GET - метод, который получает типы дефектов
        public Task<List<DTODefectType>> GetDefectTypes()
        {
            List<DefectType> dTypes = MyDB.db.DefectType.ToList();
            List<DTODefectType> dtoDTypes = new List<DTODefectType>();
            foreach (var el in dTypes)
            {
                dtoDTypes.Add(new DTODefectType(el.DefectId, el.DefectName));
            }
            return Task.Run(() => dtoDTypes);
        }

        // GET - метод, который получает спецификации (типы стелажей) по ResoultID
        public Task<List<Deviation>> GetDeviation(int ResoultID)
        {
            return MyDB.db.Deviation.Where(i => i.ResoultId == ResoultID).ToListAsync();
        }

        public Task<List<Element>> GetElement()
        {
            return MyDB.db.Element.ToListAsync();    
        }

        // GET - метод, который получает элементы стеллажей по ResoultID
        public Task<List<v_GetSpecificationsElement>> GetSpecificationElement(int ResoultID)
        {
            return MyDB.db.v_GetSpecificationsElement.Where(i => i.ResoultID == ResoultID).ToListAsync();
        }

        // GET - метод, который получает спецификации (типы стелажей) по ResoultID
        public Task<List<Specifications>> GetSpecifications(int ResoultID)
        {
            return MyDB.db.Specifications.Where(i => i.ResoultId == ResoultID).ToListAsync();
        }

        public Task<List<v_GetElementSize>> GetSpecificationsElementSizes(int SpecificationsElementID)
        {
            return MyDB.db.v_GetElementSize.Where(elem => elem.SpecificationsElementID == SpecificationsElementID).ToListAsync();
        }

        

        // GET - метод, который получает роли
        public Task<List<ClientPermissions>> GetUserRoles()
        {
            return MyDB.db.ClientPermissions.ToListAsync();
        }
    }
}
