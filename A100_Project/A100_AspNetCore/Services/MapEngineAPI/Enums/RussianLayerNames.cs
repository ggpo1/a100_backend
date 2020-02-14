using System;

namespace A100_AspNetCore.Services.MapEngineAPI.Enums
{
    public class RussianLayerNames
    {
        private readonly String name;
        private readonly int value;

        public static readonly RussianLayerNames WALLS = new RussianLayerNames(1, "Стены");
        public static readonly RussianLayerNames STILLAGES = new RussianLayerNames(2, "Стеллажи");
        public static readonly RussianLayerNames LIGHTING = new RussianLayerNames(3, "Освещение");
        public static readonly RussianLayerNames TEXT = new RussianLayerNames(4, "Текст");

        private RussianLayerNames(int value, String name)
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