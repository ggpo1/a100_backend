using System;

namespace A100_AspNetCore.Models.A100_Models.DataBase
{
    public class GlobalsatBangs
    {
        public int ID { get; set; }
        public string SensorID { get; set; }
        public float? Strength { get; set; }
        public string Status { get; set; }
        public DateTime BangDate { get; set; }
        public int ResoultID { get; set; }
    }
}