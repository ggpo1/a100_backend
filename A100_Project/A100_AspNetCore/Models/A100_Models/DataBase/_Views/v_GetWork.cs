﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DataBase._Views
{
    public class v_GetWork
    {
        public int ControlID { get; set; }

        public int WarhouseID { get; set; }

        public DateTime? ControlData { get; set; }

        public string Name { get; set; }

        public string InspectionMetod { get; set; }

        public int? ResoultID { get; set; }

        public string CityName { get; set; }

        public string CompanyName { get; set; }

        public string WarhouseName { get; set; }

        public string ProjectNumber { get; set; }

        public string ControlName { get; set; }
    }
}
