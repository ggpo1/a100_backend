using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API._DBService;
using Microsoft.EntityFrameworkCore;
using System.IO;
using A100_AspNetCore.Models.A100_Models.DTO;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
// using static System.Net.Mime.MediaTypeNames;

namespace A100_AspNetCore.Services.API.VikService
{
    public class VikService : IVikService
    {
        public Task<object> UpdateVik(int VikID, Vik vik)
        {
            var uptVik = MyDB.db.Vik.SingleOrDefault(el => el.VikId == VikID);

            if (uptVik != null)
            {
                uptVik.Frame = vik.Frame;
                uptVik.StructuralMemberId = vik.StructuralMemberId;
                uptVik.DefectId = vik.DefectId;
                uptVik.RiskLevelId = vik.RiskLevelId;
                uptVik.States = vik.States;
                uptVik.CComment = vik.CComment;
                uptVik.MX = vik.MX;
                uptVik.MY = vik.MY;
                uptVik.MRotation = vik.MRotation;
                uptVik.SpecificationsElementId = vik.SpecificationsElementId;
                uptVik.TransformRow = vik.TransformRow;
                uptVik.ElementOrientation = vik.ElementOrientation;
                uptVik.DamagePhoto = vik.DamagePhoto;
                uptVik.MScale = vik.MScale;
                uptVik.EmployeeId = vik.EmployeeId;
                uptVik.UpdateTime = vik.UpdateTime;
                uptVik.Row = vik.Row;
                uptVik.NLevel = vik.NLevel;
                uptVik.FrameRange = vik.FrameRange;
                uptVik.PartialToid = vik.PartialToid;
                uptVik.ShowMode = vik.ShowMode;
                uptVik.OriginalVikId = vik.OriginalVikId;
                uptVik.UniqueId = vik.UniqueId;
                uptVik.Otkmark = vik.Otkmark;
                
                MyDB.db.Entry(uptVik).State = EntityState.Modified;
                MyDB.db.SaveChanges();

                var result = MyDB.db.v_GetVik.FirstOrDefault(el => el.VikID == VikID);
                return Task.Run(() => (object)new { result.VikID, result.ResoultID });
            }

            return Task.Run(() => (object)new { Result = 0 });
        }
        public Task<CameraEvents> UpdateCameraEvent(CameraEvents cEvnt)
        {
            CameraEvents evnt = MyDB.db.CameraEvents
                .Where(elem => elem.CameraEventId == cEvnt.CameraEventId)
                .FirstOrDefault();
            evnt.ImageByte = cEvnt.ImageByte;
            MyDB.db.CameraEvents.Update(evnt);
            MyDB.db.SaveChanges();
            return Task.Run(() => evnt);
        }

        public async Task<List<DTOVikByUnit>> GetViksByPartialTO(int partialTOID)
        {
            List<Vik> vList = MyDB.db.Vik.Where(elem => elem.PartialToid == partialTOID).ToList();
            List<DTOVikByUnit> vdList = new List<DTOVikByUnit>();
            foreach (var item in vList)
            {
                

                v_GetVikByUnit vik = MyDB.db.v_GetVikByUnit.FirstOrDefault(elem => elem.VikID == item.VikId);

                var photo = GetVikPhoto((int)vik.ResoultID, vik.VikID).Result.photo;

                DTOVikByUnit dVik = new DTOVikByUnit()
                {
                    VikID = vik.VikID,
                    Row = vik.Row,
                    Frame = vik.Frame,
                    nLevel = vik.nLevel,
                    StructuralMemberID = vik.StructuralMemberID,
                    DefectID = vik.DefectID,
                    RiskLevelID = vik.RiskLevelID,
                    States = vik.States,
                    cComment = vik.cComment,
                    mX = vik.mX,
                    mY = vik.mY,
                    mRotation = vik.mRotation,
                    ResoultID = vik.ResoultID,
                    UnitName = vik.UnitName,
                    WarhouseID = vik.WarhouseID,
                    ElementID = vik.ElementID,
                    mScale = vik.mScale,
                    EmployeeID = vik.EmployeeID,
                    SpecificationsID = vik.SpecificationsID,
                    UpdateTime = vik.UpdateTime,
                    FrameRange = vik.FrameRange,
                    SpecificationsElementID = vik.SpecificationsElementID,
                    ElementCount = vik.ElementCount,
                    PartialTOID = vik.PartialTOID,
                    ShowMode = vik.ShowMode,
                    Path = vik.Path,
                    ElementOrientation = vik.ElementOrientation,
                    ManufacturedStillage = vik.ManufacturedStillage
                };
                if (photo == null)
                {
                    dVik.DamagePhoto = null;
                }
                else
                {
                    dVik.DamagePhoto = "";
                }
                vdList.Add(dVik);
            }
            return vdList;
        }

        public async Task<DTOAddVik> AddNewVik(Vik vik)
        {
            EntityEntry<Vik> saved = await MyDB.db.Vik.AddAsync(vik);
            MyDB.db.SaveChanges();
            int rID = MyDB.db.v_GetVik.FirstOrDefault(elem => elem.VikID == saved.Entity.VikId).ResoultID;
            DTOAddVik dVik = new DTOAddVik()
            {
                ResoultID = rID,
                VikID = saved.Entity.VikId
            };
            return dVik;
        }

        public Task<object> AddVikPhoto(int resoultID, int vikID, byte[] photo)
        {
            string appData = @"C:/inetpub/wwwroot/asti/Content";
            // string appData = @"C:/Users/v.arkhangelsky/Desktop";

            string photoPath = $"{appData}/{resoultID}/VIK/{vikID}.jpg";
            string dirPath = $"{appData}/{resoultID}/VIK";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(photoPath, FileMode.Create)))
                {
                    // byte[] bytes = System.Text.Encoding.ASCII.GetBytes(photo);
                    bw.Write(photo);
                    bw.Close();
                }
                return Task.Run(() => (object)new { result = true });
            }
            catch
            {
                return Task.Run(() => (object)new { result = false });
            }
        }

        public Task<List<v_GetVik>> GetHello(int ResoultID)
        {
            return MyDB.db.v_GetVik.Where(i => i.ResoultID == ResoultID).ToListAsync();
        }

        public async Task<List<DTOVikByUnit>> GetViksByUnit(int resoultID, string unitName)
        {
            var viks = await MyDB.db.v_GetVikByUnit.Where(elem => elem.UnitName == unitName && elem.ResoultID == resoultID).ToListAsync();
            List<DTOVikByUnit> dViks = new List<DTOVikByUnit>();
            if (viks.Count > 0)
            {
                foreach (var item in viks)
                {
                    
                    var photo = GetVikPhoto((int)item.ResoultID, item.VikID).Result.photo;

                    DTOVikByUnit dVik = new DTOVikByUnit()
                    {
                        VikID = item.VikID,
                        Row = item.Row,
                        Frame = item.Frame,
                        nLevel = item.nLevel,
                        StructuralMemberID = item.StructuralMemberID,
                        DefectID = item.DefectID,
                        RiskLevelID = item.RiskLevelID,
                        States = item.States,
                        cComment = item.cComment,
                        mX = item.mX,
                        mY = item.mY,
                        mRotation = item.mRotation,
                        ResoultID = item.ResoultID,
                        UnitName = item.UnitName,
                        WarhouseID = item.WarhouseID,
                        ElementID = item.ElementID,
                        mScale = item.mScale,
                        EmployeeID = item.EmployeeID,
                        SpecificationsID = item.SpecificationsID,
                        UpdateTime = item.UpdateTime,
                        FrameRange = item.FrameRange,
                        SpecificationsElementID = item.SpecificationsElementID,
                        ElementCount = item.ElementCount,
                        PartialTOID = item.PartialTOID,
                        ShowMode = item.ShowMode,
                        Path = item.Path,
                        ElementOrientation = item.ElementOrientation,
                        ManufacturedStillage = item.ManufacturedStillage
                    };
                    if (photo == null)
                    {
                        dVik.DamagePhoto = null;
                    }
                    else
                    {
                        dVik.DamagePhoto = "";
                    }
                    dViks.Add(dVik);
                }
                return dViks;
            }
            return new List<DTOVikByUnit>();
        }

        public Task<DTOPhoto> GetVikPhoto(int rid, int idVik)
        {           
            string appData = @"C:/inetpub/wwwroot/asti/Content";
            string photoPath = $"{appData}/{rid}/VIK/{idVik}.jpg";
                      
            if (File.Exists(photoPath))
            {
                FileStream objfilestream = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
                int len = (int)objfilestream.Length;
                Byte[] bImg = new Byte[len];
                objfilestream.Read(bImg, 0, len);
                objfilestream.Close();
                DTOPhoto resp = new DTOPhoto { photo = bImg };
                return Task.Run(() => resp);
            }
            DTOPhoto nullResp = new DTOPhoto { photo = null };
            return Task.Run(() => nullResp);
        }

        // GET - метод, который получает VIK по ResoultID
        public Task<List<v_GetVik>> GetVik(int ResoultID)
        {
            return MyDB.db.v_GetVik.Where(i => i.ResoultID == ResoultID).ToListAsync();
        }

        // GET - метод, который получает названия VIK
        public Task<List<VikElement>> GetVikNames()
        {
            return MyDB.db.VikElement.ToListAsync();
        }

        

        public Task<CameraEvents> GetCameraEvents(string userName)
        {
            return MyDB.db.CameraEvents.FirstOrDefaultAsync(elem => elem.UserName == userName);
        }
        public Task<string> DeleteCameraEvents(string username)
        {
            CameraEvents ev = MyDB.db.CameraEvents
                .Where(elem => elem.UserName == username)
                .FirstOrDefault();

            MyDB.db.CameraEvents.Remove(ev);
            MyDB.db.SaveChanges();
            return Task.Run(() => "ok");
        }

        public Task<List<byte[]>> GetVIKPhotoList(int idVik)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterCameraEvent(CameraEvents camEvent)
        {
            CameraEvents camEv = new CameraEvents()
            {
                UserName = camEvent.UserName,
                Done = null
            };
            MyDB.db.CameraEvents.Add(camEv);
            MyDB.db.SaveChanges();
            return Task.Run(() => "ok");
        }

        
    }
}
