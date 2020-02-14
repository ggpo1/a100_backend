using A100_AspNetCore.Services.MapEngineAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class VikItem
    {
        public int Id { get; set; }
        public int Place { get; set; }

        public string Row { get; set; }
        public string ElementName { get; set; }
        public int Level { get; set; }
        public string ElementSize { get; set; }
        public string Color { get; set; }
        public string ElementManufacturer { get; set; }
        public string DefectType { get; set; }
        public string Comment { get; set; }
        public bool IsRepaired { get; set; }
        public string DefectDate { get; set; }
        public string RepairDate { get; set; }
        public int DetailsCount { get; set; }
        public List<byte[]> DefectPhotos { get; set; }
        public List<byte[]> RepairsPhotos { get; set; }
        public int StillageID { get; set; }
    }
}
