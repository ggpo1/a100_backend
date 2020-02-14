using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public class Orientation
    {
        private readonly String name;
        private readonly int value;

        public static readonly Orientation HORIZONTAL = new Orientation(1, "horizontal");
        public static readonly Orientation VERTICAL = new Orientation(2, "vertical");

        private Orientation(int value, String name)
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
