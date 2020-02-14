using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTODefectType
    {
        public DTODefectType(int defectId, string defectName)
        {
            DefectId = defectId;
            DefectName = defectName ?? throw new ArgumentNullException(nameof(defectName));
        }

        public int DefectId { get; set; }
        public string DefectName { get; set; }
    }
}
