using System;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class v_GetBang
    {
        public int ID { get; set; }
        public string SensorID { get; set; }
        public double? Strength { get; set; }
        public string Place { get; set; }
        public string Row { get; set; }
        public string UnitName { get; set; }
        public DateTime BangDate { get; set; }
        public string Status { get; set; }
        public int ResoultID { get; set; }
    }
}