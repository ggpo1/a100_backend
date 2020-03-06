using System.Collections.Generic;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.DTO
{
    public class GetUnitsDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public List<object> Layers { get; set; }
    }
}