using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.API.PartialService
{
    public interface IPartialService
    {
        Task<List<PartialTo>> GetPartialTo();
        Task<List<DTOPartial>> GetPartialToByResoultID(int ResoultID);
        Task<PartialTo> AddNewPartialTo(PartialTo partialTo);
        Task<object> AddNewPartialProgress(List<PartialTOProgress> progress);
        Task<object> StopPartialTO(int partialTOID);
    }
}
