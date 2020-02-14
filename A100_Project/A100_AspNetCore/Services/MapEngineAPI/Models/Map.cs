using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models
{
    public class Map
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public List<MapLayer> Layers { get; set; } 
    }
}
