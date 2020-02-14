using A100_AspNetCore.Models.A100_Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTOPartial
    {
        public DTOPartial(int partialToid, DateTime partialTodate, int warhouseId, string employeeName, bool? transformRow, DateTime? updateTime, int resoultId, bool? isOver)
        {
            PartialToid = partialToid;
            PartialTodate = partialTodate;
            WarhouseId = warhouseId;
            EmployeeName = employeeName;
            TransformRow = transformRow;
            UpdateTime = updateTime;
            ResoultId = resoultId;
            IsOver = isOver;
        }

        public int PartialToid { get; set; }
        public DateTime PartialTodate { get; set; }
        public int WarhouseId { get; set; }
        public string EmployeeName { get; set; }
        public bool? TransformRow { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int ResoultId { get; set; }
        public bool? IsOver { get; set; }
        public List<PartialTOProgress> PartialTOElements { get; set; }
    }
}
