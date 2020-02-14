using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class ObjectItem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public dynamic Photo { get; set; }
        public string Description { get; set; }
    }
}
