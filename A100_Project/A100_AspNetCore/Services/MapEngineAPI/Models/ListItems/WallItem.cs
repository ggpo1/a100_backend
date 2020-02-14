using A100_AspNetCore.Services.MapEngineAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class WallItem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Length { get; set; }
        public string Orientation { get; set; }
        public string Color { get; set; }
    }
}
