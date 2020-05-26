using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace A100_AspNetCore.Models.A100_Models.DataBase
{
    public class GlobalsatSensors
    {
        public int ID { get; set; }
        public string ResoultID { get; set; }
        public int SensorID { get; set; }
        public string Place1 { get; set; }
        public string Place2 { get; set; }
        public string Row { get; set; }
    }
}