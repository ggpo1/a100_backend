using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class v_GetElementSize
    {
        public int SpecificationsElementID { get; set; }

        public int ElementID { get; set; }

        public string ElementName { get; set; }

        public string ElementSize { get; set; }
    }
}
