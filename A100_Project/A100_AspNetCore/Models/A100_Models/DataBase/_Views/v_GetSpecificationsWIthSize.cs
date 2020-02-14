using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class v_GetSpecificationsWIthSize
    {
        public int SpecificationsID { get; set; }

        public int SpecificationsElementID { get; set; }

        public int ElementID { get; set; }

        public string ElementName { get; set; }
        
        public string ElementSize { get; set; }
    }
}
