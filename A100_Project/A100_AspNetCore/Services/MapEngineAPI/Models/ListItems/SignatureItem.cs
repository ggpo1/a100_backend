using A100_AspNetCore.Services.MapEngineAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class SignatureItem
    {
        public string Title { get; set; }
        public string Position { get; set; }
        public int StillageID { get; set; }
    }
}
