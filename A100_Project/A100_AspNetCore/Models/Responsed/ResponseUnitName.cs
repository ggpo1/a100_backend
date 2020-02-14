using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonLibrary;

namespace A100_AspNetCore.Models.Responsed
{
    public class ResponseUnitName : IResponsed
    {
        public string UnitName { get; set; }

        public string ToJson()
        {
            return JsonFunctional.GetJson(this);
        }
    }
}
