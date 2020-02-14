using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class ElementSize
    {
        public ElementSize(int specificationsElementID, string size)
        {
            SpecificationsElementID = specificationsElementID;
            this.size = size ?? throw new ArgumentNullException(nameof(size));
        }

        public int SpecificationsElementID { get; set; }
        public string size { get; set; }
    }
}
