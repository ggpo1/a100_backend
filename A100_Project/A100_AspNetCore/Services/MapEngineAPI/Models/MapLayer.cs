using A100_AspNetCore.Services.MapEngineAPI.Enums;
using A100_AspNetCore.Services.MapEngineAPI.Models.ListItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models
{
    public class MapLayer
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public MapIconsType MapIconsType { get; set; }
        public List<ObjectItem> Objects { get; set; }
        public List<StillageItem> Stillages { get; set; }
        public List<WallItem> Walls { get; set; }
        public List<TextItem> Texts { get; set; }
    }
}
