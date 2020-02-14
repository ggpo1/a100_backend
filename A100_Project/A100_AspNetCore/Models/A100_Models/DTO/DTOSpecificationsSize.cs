using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTOSpecificationsSize
    {
        public DTOSpecificationsSize(int elementID, string elementName, List<ElementSize> elementSizes)
        {
            ElementID = elementID;
            ElementName = elementName ?? throw new ArgumentNullException(nameof(elementName));
            ElementSizes = elementSizes ?? throw new ArgumentNullException(nameof(elementSizes));
        }
        public int SpecificationsElementID { get; set; }

        public int ElementID { get; set; }

        public string ElementName { get; set; }

        public List<ElementSize> ElementSizes { get; set; }

        
    }
}
