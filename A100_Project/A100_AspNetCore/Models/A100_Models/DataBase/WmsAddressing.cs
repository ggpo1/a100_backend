using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DataBase
{
    public class WmsAddressing
    {
        public int ID { get; set; }
        public string A100Row { get; set; }
        public string WmsRow { get; set; }
        public int ResoultID { get; set; }
        public string MapUnit { get; set; }
    }
}
