using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTOWmsGetViks
    {
        public DTOWmsGetViks(int vikID, 
            string row, 
            int place, 
            string level, 
            string riskLevel, 
            string elementName, 
            string comment, 
            string elementOrientation, 
            string stillageTypeName, 
            string unitName, 
            int? resoultID)
        {
            VikID = vikID;
            Row = row;
            Place = place;
            Level = level;
            RiskLevel = riskLevel;
            ElementName = elementName;
            Comment = comment;
            ElementOrientation = elementOrientation;
            StillageTypeName = stillageTypeName;
            UnitName = unitName;
            ResoultID = resoultID;
        }

        public DTOWmsGetViks() { }

        public int VikID { get; set; }
        public string Row { get; set; }
        public int Place { get; set; }
        public string Level { get; set; }
        public string RiskLevel { get; set; }
        public string ElementName { get; set; }
        public string Comment { get; set; }
        public string ElementOrientation { get; set; }
        public string StillageTypeName { get; set; }
        public string UnitName { get; set; }
        public int? ResoultID { get; set; }
    }
}
