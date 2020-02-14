using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public class StillageSize
    {
        private readonly String name;
        private readonly int value;

        public static readonly StillageSize NORMAL = new StillageSize(1, "normal");
        public static readonly StillageSize SMALL = new StillageSize(2, "small");

        private StillageSize(int value, String name)
        {
            this.name = name;
            this.value = value;
        }

        public override String ToString()
        {
            return name;
        }
    }
}
