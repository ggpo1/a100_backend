using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A100_AspNetCore.Services.Globalsat.Models.DTO;

namespace A100_AspNetCore.Services.Globalsat.GlobalsatService
{
    public interface IGlobalsatService
    {
        // Task<Object> AddSensors();
        // Task<Object> AddRacks();
        Task<Object> AddBang(AddBangRequest data);
        Task<Object> AddDeviations(List<AddDeviationsRequest> data);
    }
}