using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public class DefectColors
    {
        private readonly String name;
        private readonly int value;

        public static readonly DefectColors RED = new DefectColors(1, "#FF003C");
        public static readonly DefectColors YELLOW = new DefectColors(2, "#FFBB00");
        public static readonly DefectColors GREEN = new DefectColors(2, "#06F107");

        private DefectColors(int value, String name)
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
