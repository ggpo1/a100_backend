using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public sealed class LayerType
    {

        private readonly String name;
        private readonly int value;

        public static readonly LayerType ABSTRACTS = new LayerType(1, "abstract");
        public static readonly LayerType STILLAGES = new LayerType(2, "stillages");
        public static readonly LayerType SIGNATURES = new LayerType(3, "signatures");
        public static readonly LayerType WALLS = new LayerType(4, "walls");
        public static readonly LayerType LIGHTING = new LayerType(5, "lighting");
        public static readonly LayerType TEXT = new LayerType(6, "text");

        private LayerType(int value, String name)
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
