using System;

namespace A100_AspNetCore.Models.A100_Models.DataBase
{
    public class GlobalsatDeviations
    {
        public int ID { get; set; }
        public string SensorID { get; set; }
        public float? DeviationValue { get; set; }
        public DateTime DeviationDate { get; set; }
        public int ResoultID { get; set; }
    }
}