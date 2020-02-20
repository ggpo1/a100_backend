using A100_AspNetCore.Services.MapEngineAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;

namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class StillageItem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Size { get; set; }
        public string Orientation { get; set; }
        public List<VikItem> Viks { get; set; }
        public List<DeviationItem> Deviations { get; set; }
        public List<PlaceSignatureItem> PlaceSignatures { get; set; }
        public SignatureItem Signature { get; set; }
        public decimal PmCount { get; set; }
        public int Scale { get; set; }
        public bool IsBlockScaling { get; set; }
        // public int SpecificationsElementId { get; set; }
    }
    
    // public set SetSignature(SignatureItem)
    // {
        
    // }

}
