using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public class SignaturePosition
    {
        private readonly String name;
        private readonly int value;

        public static readonly SignaturePosition TOP = new SignaturePosition(1, "top");
        public static readonly SignaturePosition LEFT = new SignaturePosition(2, "left");
        public static readonly SignaturePosition RIGHT = new SignaturePosition(2, "right");
        public static readonly SignaturePosition BOTTOM = new SignaturePosition(2, "bottom");

        private SignaturePosition(int value, String name)
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
