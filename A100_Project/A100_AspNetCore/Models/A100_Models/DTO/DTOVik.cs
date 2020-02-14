using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTOVik
    {
        public DTOVik(int vikId, int frame, int? structuralMemberId, int? defectId, int? riskLevelId, int? states, string cComment, decimal? mX, decimal? mY, decimal? mRotation, int? specificationsElementId, int? transformRow, string elementOrientation, string damagePhoto, int? mScale, int? employeeId, DateTime? updateTime, string row, string nLevel, string frameRange, int? partialToid, byte? showMode, int? originalVikId, int? uniqueId, byte? otkmark, string unitName, string manufacturedStillage)
        {
            VikId = vikId;
            Frame = frame;
            StructuralMemberId = structuralMemberId ?? throw new ArgumentNullException(nameof(structuralMemberId));
            DefectId = defectId ?? throw new ArgumentNullException(nameof(defectId));
            RiskLevelId = riskLevelId ?? throw new ArgumentNullException(nameof(riskLevelId));
            States = states ?? throw new ArgumentNullException(nameof(states));
            CComment = cComment ?? throw new ArgumentNullException(nameof(cComment));
            MX = mX ?? throw new ArgumentNullException(nameof(mX));
            MY = mY ?? throw new ArgumentNullException(nameof(mY));
            MRotation = mRotation ?? throw new ArgumentNullException(nameof(mRotation));
            SpecificationsElementId = specificationsElementId ?? throw new ArgumentNullException(nameof(specificationsElementId));
            TransformRow = transformRow ?? throw new ArgumentNullException(nameof(transformRow));
            ElementOrientation = elementOrientation ?? throw new ArgumentNullException(nameof(elementOrientation));
            DamagePhoto = damagePhoto;
            MScale = mScale ?? throw new ArgumentNullException(nameof(mScale));
            EmployeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            UpdateTime = updateTime ?? throw new ArgumentNullException(nameof(updateTime));
            Row = row ?? throw new ArgumentNullException(nameof(row));
            NLevel = nLevel ?? throw new ArgumentNullException(nameof(nLevel));
            FrameRange = frameRange ?? throw new ArgumentNullException(nameof(frameRange));
            PartialToid = partialToid ?? throw new ArgumentNullException(nameof(partialToid));
            ShowMode = showMode ?? throw new ArgumentNullException(nameof(showMode));
            OriginalVikId = originalVikId ?? throw new ArgumentNullException(nameof(originalVikId));
            UniqueId = uniqueId ?? throw new ArgumentNullException(nameof(uniqueId));
            Otkmark = otkmark ?? throw new ArgumentNullException(nameof(otkmark));
            UnitName = unitName;
            ManufacturedStillage = manufacturedStillage;
        }

        public DTOVik(int vikId, int frame, int? structuralMemberId, int? defectId, int? riskLevelId, int? states, string cComment, decimal? mX, decimal? mY, decimal? mRotation, int? specificationsElementId, int? transformRow, string elementOrientation, string damagePhoto, int? mScale, int? employeeId, DateTime? updateTime, string row, string nLevel, string frameRange, int? partialToid, int? originalVikId, int? uniqueId, byte? otkmark, string unitName, string manufacturedStillage)
        {
            VikId = vikId;
            Frame = frame;
            StructuralMemberId = structuralMemberId;
            DefectId = defectId;
            RiskLevelId = riskLevelId;
            States = states;
            CComment = cComment;
            MX = mX;
            MY = mY;
            MRotation = mRotation;
            SpecificationsElementId = specificationsElementId;
            TransformRow = transformRow;
            ElementOrientation = elementOrientation;
            DamagePhoto = damagePhoto;
            MScale = mScale;
            EmployeeId = employeeId;
            UpdateTime = updateTime;
            Row = row;
            NLevel = nLevel;
            FrameRange = frameRange;
            PartialToid = partialToid;
            OriginalVikId = originalVikId;
            UniqueId = uniqueId;
            Otkmark = otkmark;
            UnitName = unitName;
            ManufacturedStillage = manufacturedStillage;
        }

        public int VikId { get; set; }
        public int Frame { get; set; }
        public int? StructuralMemberId { get; set; }
        public int? DefectId { get; set; }
        public int? RiskLevelId { get; set; }
        public int? States { get; set; }
        public string CComment { get; set; }
        public decimal? MX { get; set; }
        public decimal? MY { get; set; }
        public decimal? MRotation { get; set; }
        public int? SpecificationsElementId { get; set; }
        public int? TransformRow { get; set; }
        public string ElementOrientation { get; set; }
        public string DamagePhoto { get; set; }
        public int? MScale { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Row { get; set; }
        public string NLevel { get; set; }
        public string FrameRange { get; set; }

        public static explicit operator DTOVik(Vik v)
        {
            throw new NotImplementedException();
        }

        public int? PartialToid { get; set; }
        public byte? ShowMode { get; set; }
        public int? OriginalVikId { get; set; }
        public int? UniqueId { get; set; }
        public byte? Otkmark { get; set; }
        public string UnitName { get; set; }
        public string ManufacturedStillage { get; set; }

        public static DTOVik ToDTOVik(Vik vik)
        {
            return new DTOVik(vik.VikId, vik.Frame,
                vik.StructuralMemberId, vik.DefectId,
                vik.RiskLevelId, vik.States,
                vik.CComment, vik.MX,
                vik.MY, vik.MRotation,
                vik.SpecificationsElementId, vik.TransformRow,
                vik.ElementOrientation, null,
                vik.MScale, vik.EmployeeId,
                vik.UpdateTime, vik.Row,
                vik.NLevel, vik.FrameRange,
                vik.PartialToid, vik.OriginalVikId,
                vik.UniqueId, vik.Otkmark,
                null, null);
        }
    }
}
