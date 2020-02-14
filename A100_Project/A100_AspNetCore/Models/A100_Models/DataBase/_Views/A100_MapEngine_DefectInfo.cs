using System;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class A100_MapEngine_DefectInfo
    {
        public int VikID { get; set; }
        public string nLevel { get; set; }
        public string Row { get; set; }
        public string ElementName { get; set; }
        public string ElementSize { get; set; }
        public string RiscLevelName { get; set; }
        public string DefectName { get; set; }
        public string ManufacturedStillage { get; set; }
        public string cComment { get; set; }
        public int States { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? RepairDate { get; set; }
        public int ResoultID { get; set; }
    }
}