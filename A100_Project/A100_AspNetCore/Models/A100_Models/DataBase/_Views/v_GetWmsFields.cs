using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class v_GetWmsFields
    {
        public int ID { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string SensorID { get; set; }
        public int ResoultID { get; set; }
        public string UnitName { get; set; }
    }
}
