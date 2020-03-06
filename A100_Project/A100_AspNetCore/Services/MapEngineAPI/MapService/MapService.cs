using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Services.API._DBService;
using A100_AspNetCore.Services.MapEngineAPI.Enums;
using A100_AspNetCore.Services.MapEngineAPI.Models;
using A100_AspNetCore.Services.MapEngineAPI.Models.DTO;
using A100_AspNetCore.Services.MapEngineAPI.Models.ListItems;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Map = A100_AspNetCore.Services.MapEngineAPI.Models.Map;
using Orientation = A100_AspNetCore.Services.MapEngineAPI.Enums.Orientation;

namespace A100_AspNetCore.Services.MapEngineAPI.MapService {
    public class MapService : IMapService {
        public List<int> WallsIndexes = new List<int> () {-3, -4, -5, -1 };

        public async Task<List<GetUnitsDTO>> GetUnitNamesByResoult (int ResoultID) {
            List<v_GetUnitNamesByResoult> notValidUnits = await MyDB.db.v_GetUnitNamesByResoult.Where (el => el.ResoultID == ResoultID).ToListAsync ();
            List<GetUnitsDTO> validUnits = new List<GetUnitsDTO> ();
            int i = 1;
            notValidUnits.ForEach (el => {
                validUnits.Add (new GetUnitsDTO () {
                    Id = i,
                        Key = "unit_" + i,
                        Title = el.UnitName,
                        Layers = new List<object> ()
                });
                i++;
            });
            return await Task.Run (() => validUnits);
        }

        // public async Task<List<string>> GetTSXByUnitName(int ResoultID, string UnitName, string UnitKey)
        // {

        //     return await Task.Run(() => new List<string>() {""});
        // }

        public async Task<List<MapLayer>> GetUnitLayersByUnitName (int ResoultID, string UnitName, string UnitKey) {
            List<MapLayer> list = new List<MapLayer> ();

            List<v_GetMap> _stillages = await MyDB.db.v_GetMap.Where (i => (i.ResoultID == ResoultID) && (i.MapUnit == UnitName) && (i.cIndex >= 0)).ToListAsync ();
            List<v_GetMap> _walls = await MyDB.db.v_GetMap.Where (i =>
                (i.ResoultID == ResoultID) &&
                (i.MapUnit == UnitName) &&
                (WallsIndexes.Contains ((int) i.cIndex))).ToListAsync ();
            List<A100_MapEngine_DefectInfo> vikInfos = await
            MyDB.db.A100_MapEngine_DefectInfo.Where (el => el.ResoultID == ResoultID).ToListAsync ();
            List<v_GetMap> _texts = await MyDB.db.v_GetMap.Where (i => (i.ResoultID == ResoultID) && (i.MapUnit == UnitName) && (i.cIndex == -2)).ToListAsync ();
            List<v_GetVikByUnit> viks = await MyDB.db.v_GetVikByUnit.Where (vEl => vEl.ResoultID == ResoultID).ToListAsync ();
            List<Deviation> deviations = await MyDB.db.Deviation.Where (el => el.ResoultId == ResoultID).ToListAsync ();
            List<List<StillageItem>> nabivStillagesGroups = new List<List<StillageItem>> ();
            
            int layerK = 1;
            
            if (_stillages.Count != 0) 
            {
                list.Add (new MapLayer () {
                    Id = layerK,
                        Key = UnitKey + "_layer_" + layerK,
                        Title = RussianLayerNames.STILLAGES.ToString (),
                        Type = LayerType.STILLAGES.ToString (),
                        MapIconsType = MapIconsType.DRAWING,
                });
                list[list.Count - 1].Stillages = new List<StillageItem> ();
                list[list.Count - 1].Objects = new List<ObjectItem> ();
                list[list.Count - 1].Walls = new List<WallItem> ();
                list[list.Count - 1].Texts = new List<TextItem> ();

                int sK = 0;
                bool isBlockScaling = false;
                foreach (v_GetMap el in _stillages) {

                    string _orientation = "";
                    int _x = 0;
                    int _y = 0;
                    decimal pmCount = Math.Floor ((decimal) WidthOrHeight (el.Width, el.Height));
                    int scale = el.Width < el.Height ? (int) el.Width : (int) el.Height;
                    int iteration = 0;
                    Int32.TryParse (el.Iteration.ToString (), out iteration);

                    foreach (var st in _stillages) {
                        int _scale = st.Width < st.Height ? (int) st.Width : (int) st.Height;
                        if (_scale > 1) isBlockScaling = true;
                    }

                    if (el.X != null) _x = ((int) el.X * 10);
                    if (el.Y != null) _y = ((int) el.Y * 10);

                    if (el.Width > el.Height) _orientation = Orientation.HORIZONTAL.ToString ();
                    else if (el.Height > el.Width) _orientation = Orientation.VERTICAL.ToString ();
                    else if (el.Width == el.Height) _orientation = Orientation.HORIZONTAL.ToString ();

                    if (scale == 1) 
                    {
                        if (_orientation == Orientation.VERTICAL.ToString ()) {
                            _x *= isBlockScaling ? 6 : 3;
                            _y *= isBlockScaling ? 6 : 3;
                        } else if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                            _x *= isBlockScaling ? 6 : 3;
                            _y *= isBlockScaling ? 6 : 3;
                        }
                    } 
                    else 
                    {
                        if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                            _x *= 6;
                            _y *= 6;
                        } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                            _x *= 6;
                            // _x += (Convert.ToInt32(el.Width) * 30);
                            _y *= 6;
                        }
                    }

                    
                    Specifications spec =
                        await MyDB.db.Specifications.FirstOrDefaultAsync (specEl =>
                            specEl.SpecificationsId == el.SpecificationsID);
                    
                    if (spec.StillageTypeId == 5) { 
                        // проверка набивных _______________________________________________________________________
                        List<StillageItem> nStillages = new List<StillageItem>();

                        int needListIndex = -1;
                        int k = 0;
                        foreach (List<StillageItem> checkGroup in nabivStillagesGroups)
                        {
                            foreach (StillageItem checkGroupItem in checkGroup)
                                if (checkGroupItem.Row == el.Row) needListIndex = k;
                            k++;
                        }

                        if (needListIndex == -1)
                        {
                            // добавление нового ряда набивных
                            nabivStillagesGroups.Add(new List<StillageItem>() { new StillageItem()
                            {
                                Id = el.MapID,
                                Key = list[list.Count - 1].Key + "_stillage_" + el.MapID,
                                X = _x,
                                Y = _y,
                                Size = StillageSize.NORMAL.ToString (),
                                Orientation = _orientation,
                                PmCount = pmCount,
                                Scale = scale,
                                IsBlockScaling = isBlockScaling,
                                Deviations = new List<DeviationItem>(), // TODO: add deviations calculations
                                Viks = new List<VikItem>(),
                                PlaceSignatures = new List<PlaceSignatureItem>(),
                                Row = el.Row
                            }});
                        }
                        else
                        {
                            // добавление секции к уже существующему ряду набивных
                            nabivStillagesGroups[needListIndex].Add(new StillageItem()
                            {
                                Id = el.MapID,
                                Key = list[list.Count - 1].Key + "_stillage_" + el.MapID,
                                X = _x,
                                Y = _y,
                                Size = StillageSize.NORMAL.ToString (),
                                Orientation = _orientation,
                                PmCount = pmCount,
                                Scale = scale,
                                IsBlockScaling = isBlockScaling,
                                Deviations = new List<DeviationItem>(), // TODO: add deviations calculations
                                Row = el.Row
                            });
                        } 
                        // ________________________________________________________________________________________

                    } else {

                        // отклонения
                        List<Deviation> stillageFirstSideDeviations = new List<Deviation> ();
                        List<Deviation> stillageSecondSideDeviations = new List<Deviation> ();
                        /*
                        stillageDeviations = deviations.Where (
                            dEl => dEl.Row == el.Row &&
                            dEl.Frame == el.Frame &&
                            dEl.SpecificationsId == el.SpecificationsID
                        ).ToList ();
                        */

                        stillageFirstSideDeviations = deviations.Where (devEl =>
                            devEl.ResoultId == ResoultID &&
                            devEl.MX == el.X &&
                            devEl.MY == el.Y &&
                            devEl.SpecificationsId == el.SpecificationsID
                        ).ToList ();

                        if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                            stillageSecondSideDeviations = deviations.Where (devEl =>
                                devEl.ResoultId == ResoultID &&
                                devEl.MX == (el.X + el.Width) &&
                                devEl.MY == el.Y &&
                                devEl.SpecificationsId == el.SpecificationsID
                            ).ToList ();
                        } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                            stillageSecondSideDeviations = deviations.Where (devEl =>
                                devEl.ResoultId == ResoultID &&
                                devEl.MX == el.X &&
                                devEl.MY == (el.Y + el.Height) &&
                                devEl.SpecificationsId == el.SpecificationsID
                            ).ToList ();
                        }

                        List<DeviationItem> validDeviations = new List<DeviationItem> ();

                        foreach (var dEl in stillageSecondSideDeviations) {
                            var arrowDirection = dEl.ArrowDirection;
                            validDeviations.Add (new DeviationItem () {
                                Id = dEl.DeviationId,
                                    Key = list[list.Count - 1].Key + "_stillage_" + el.MapID + "_deviation_" +
                                    dEl.DeviationId,
                                    StillageID = el.MapID
                            });

                            if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                                validDeviations[validDeviations.Count - 1].DeviationPosition =
                                    SignaturePosition.RIGHT.ToString ();
                            } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                                validDeviations[validDeviations.Count - 1].DeviationPosition =
                                    SignaturePosition.BOTTOM.ToString ();
                            }

                            if (arrowDirection == 4 || arrowDirection == 1) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = false;
                            } else if (arrowDirection == 2 || arrowDirection == 3) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = true;
                            }
                        }

                        foreach (var dEl in stillageFirstSideDeviations) {
                            var arrowDirection = dEl.ArrowDirection;
                            var deviationLocation = dEl.DeviationLocationId;
                            validDeviations.Add (new DeviationItem () {
                                Id = dEl.DeviationId,
                                    Key = list[list.Count - 1].Key + "_stillage_" + el.MapID + "_deviation_" +
                                    dEl.DeviationId,
                                    StillageID = el.MapID
                            });

                            if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                                validDeviations[validDeviations.Count - 1].DeviationPosition =
                                    SignaturePosition.LEFT.ToString ();
                            } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                                validDeviations[validDeviations.Count - 1].DeviationPosition =
                                    SignaturePosition.TOP.ToString ();
                            }

                            if (arrowDirection == 4 || arrowDirection == 1) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = false;
                            } else if (arrowDirection == 2 || arrowDirection == 3) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = true;
                            }
                        }

                        list[list.Count - 1].Stillages.Add(new StillageItem () {
                            Id = el.MapID,
                            Key = list[list.Count - 1].Key + "_stillage_" + el.MapID,
                            X = _x,
                            Y = _y,
                            Size = StillageSize.NORMAL.ToString (),
                            Orientation = _orientation,
                            PmCount = pmCount,
                            Scale = scale,
                            IsBlockScaling = isBlockScaling,
                            Deviations = validDeviations,
                            Row = el.Row
                        });
                        // 
                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].Viks =
                            new List<VikItem> ();
                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].PlaceSignatures =
                            new List<PlaceSignatureItem> ();
                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].Signature =
                            new SignatureItem ();

                        var pSignatures = new List<PlaceSignatureItem> ();

                        if (Math.Abs (iteration) < 100) {
                            // int iterK = 0;
                            // if (iteration != 0) iterK = Math.Abs(iteration % 100);
                            if (scale != 0) {
                                for (int i = 0; i < pmCount / scale; i++) {
                                    // ?????
                                    int frame = 0;
                                    Int32.TryParse (el.Frame.ToString (), out frame);

                                    var temp = iteration > 0 ?
                                        (frame + Math.Abs (iteration) * i).ToString () :
                                        (frame - Math.Abs (iteration) * i).ToString ();
                                    pSignatures.Add (
                                        new PlaceSignatureItem () 
                                        {
                                            Place = i + 1,
                                            Title = temp,
                                            StillageID = el.MapID,
                                        }
                                    );
                                }
                            }
                        } else {
                            if (scale != 0) {
                                for (int i = 0; i < pmCount / scale; i++) {
                                    // ?????
                                    int frame = 0;
                                    Int32.TryParse (el.Frame.ToString (), out frame);

                                    pSignatures.Add (
                                        new PlaceSignatureItem () 
                                        {
                                            Place = i + 1,
                                            Title = frame.ToString (),
                                            StillageID = el.MapID,
                                        }
                                    );
                                }
                            }
                        }

                        var signature = new SignatureItem () 
                        {
                            StillageID = el.MapID,
                            Title = el.Row,
                            Position = _orientation == Orientation.HORIZONTAL.ToString () ?
                            SignaturePosition.BOTTOM.ToString () :
                            SignaturePosition.RIGHT.ToString ()
                        };

                        // Повреждения
                        List<v_GetVikByUnit> stillageViks = viks.Where (stVikEl =>
                            stVikEl.Row == el.Row &&
                            stVikEl.Frame == el.Frame &&
                            stVikEl.SpecificationsID == el.SpecificationsID
                            // stVikEl.States == 0 &&
                            // stVikEl.ShowMode == 0
                            // stVikEl.PartialTOID == null
                        ).ToList ();

                        List<VikItem> validDefects = new List<VikItem> ();

                        string projectPhotoPath = @"C:\inetpub\wwwroot\asti\Content\" + ResoultID + @"\VIK\";

                        for (int iter = 0; iter < stillageViks.Count; iter++) {
                            string color = "";
                            int place = 0;

                            if (_orientation == Orientation.VERTICAL.ToString ())
                                place = Math.Abs ((int) el.Y - (int) stillageViks[iter].mY);
                            else if (_orientation == Orientation.HORIZONTAL.ToString ())
                                place = Math.Abs ((int) el.X - (int) stillageViks[iter].mX);

                            if (stillageViks[iter].RiskLevelID == 1) color = "#06F107";
                            else if (stillageViks[iter].RiskLevelID == 2) color = "#FFBB00";
                            else if (stillageViks[iter].RiskLevelID == 3) color = "#FF003C";

                            var vikInfo = vikInfos.FirstOrDefault (pred => pred.VikID == stillageViks[iter].VikID);

                            string photoPath = projectPhotoPath + stillageViks[iter].VikID;
                            List<byte[]> photos = new List<byte[]> ();
                            for (int photoIter = 0; photoIter < 4; photoIter++) {
                                var _path = photoPath + '_' + (photoIter + 1) + ".jpg";
                                if (File.Exists (_path)) {
                                    photos.Add (File.ReadAllBytes (_path));
                                }
                            }

                            if (File.Exists (photoPath + ".jpg")) {
                                photos.Add (File.ReadAllBytes (photoPath + ".jpg"));
                            }

                            photoPath = projectPhotoPath + "R_" + stillageViks[iter].VikID;
                            List<byte[]> repairsPhotos = new List<byte[]> ();
                            for (int rPhotoIter = 0; rPhotoIter < 4; rPhotoIter++) {
                                var _path = photoPath + '_' + (rPhotoIter + 1) + ".jpg";
                                if (File.Exists (_path)) {
                                    repairsPhotos.Add (File.ReadAllBytes (_path));
                                }
                            }

                            if (File.Exists (photoPath + ".jpg")) {
                                repairsPhotos.Add (File.ReadAllBytes (photoPath + ".jpg"));
                            }

                            validDefects.Add (new VikItem () {
                                Id = stillageViks[iter].VikID,
                                    Place = place == 0 ? 1 : place + 1,
                                    Row = vikInfo.Row,
                                    ElementName = vikInfo.ElementName,
                                    Level = Convert.ToInt32 (stillageViks[iter].nLevel),
                                    ElementSize = vikInfo.ElementSize,
                                    Color = color,
                                    ElementManufacturer = vikInfo.ManufacturedStillage,
                                    DefectType = vikInfo.DefectName,
                                    Comment = vikInfo.cComment,
                                    IsRepaired = vikInfo.States == 1,
                                    DefectDate = vikInfo.UpdateTime.ToString (),
                                    RepairDate = vikInfo.RepairDate.ToString (),
                                    DetailsCount = 0,
                                    DefectPhotos = photos,
                                    RepairsPhotos = repairsPhotos,
                                    StillageID = el.MapID
                            });
                        }

                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].PlaceSignatures =
                            pSignatures;

                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].Viks = validDefects;

                        list[list.Count - 1].Stillages[list[list.Count - 1].Stillages.Count - 1].Signature = signature;

                        sK++;
                    }
                }
                if (_walls.Count != 0) {
                    list.Add (new MapLayer () {
                        Id = layerK,
                            Key = UnitKey + "_layer_" + layerK,
                            Title = RussianLayerNames.WALLS.ToString (),
                            Type = LayerType.WALLS.ToString (),
                            MapIconsType = MapIconsType.DRAWING,
                    });
                    list[list.Count - 1].Stillages = new List<StillageItem> ();
                    list[list.Count - 1].Objects = new List<ObjectItem> ();
                    list[list.Count - 1].Walls = new List<WallItem> ();
                    list[list.Count - 1].Texts = new List<TextItem> ();

                    List<WallItem> walls = new List<WallItem> ();

                    int sizeK = isBlockScaling ? 6 : 3;

                    foreach (v_GetMap el in _walls) {
                        int _startX;
                        int _startY;

                        if (el.cIndex is - 4) { // горизонтальная стена
                            _startX = (int) el.X * 10;
                            _startY = (int) el.Y * 10;
                            if (el.Width < 0) {
                                _startX = (int) el.X * 10 - Math.Abs ((int) el.Width * 10);
                            }

                            walls.Add (new WallItem () {
                                Id = el.MapID,
                                    Key = list[list.Count - 1].Key +
                                    "_wall_" + el.MapID,
                                    StartX = _startX * sizeK,
                                    StartY = _startY * sizeK,
                                    Length = Math.Abs ((int) el.Width) * 10 * sizeK,
                                    Orientation = Orientation.HORIZONTAL.ToString (),
                                    Color = ""
                            });
                        } else if (el.cIndex is - 3) { // вертикальная стена
                            _startX = (int) el.X * 10;
                            _startY = (int) el.Y * 10;
                            if (el.Width < 0) {
                                _startY = (int) el.Y * 10 - Math.Abs ((int) el.Width * 10);
                            }

                            walls.Add (new WallItem () {
                                Id = el.MapID,
                                    Key = list[list.Count - 1].Key +
                                    "_wall_" + el.MapID,
                                    StartX = _startX * sizeK + 7,
                                    StartY = _startY * sizeK,
                                    Length = Math.Abs ((int) el.Width) * 10 * sizeK,
                                    Orientation = Orientation.VERTICAL.ToString (),
                                    Color = ""
                            });
                        } else if (el.cIndex is - 1) { // колонны
                            walls.Add (new WallItem () {
                                Id = el.MapID,
                                    Key = list[list.Count - 1].Key +
                                    "_wall_" + el.MapID,
                                    StartX = (int) el.X * 10 * sizeK + 30,
                                    StartY = (int) el.Y * 10 * sizeK + 30,
                                    Length = 10,
                                    Orientation = Orientation.HORIZONTAL.ToString (),
                                    Color = "black"
                            });
                        }
                    }

                    list[list.Count - 1].Walls = walls;
                }

                layerK++;
                if (_texts.Count != 0) {
                    list.Add (new MapLayer () {
                        Id = layerK,
                            Key = UnitKey + "_layer_" + layerK,
                            Title = RussianLayerNames.TEXT.ToString (),
                            Type = LayerType.TEXT.ToString (),
                            MapIconsType = MapIconsType.DRAWING,
                    });

                    list[list.Count - 1].Stillages = new List<StillageItem> ();
                    list[list.Count - 1].Objects = new List<ObjectItem> ();
                    list[list.Count - 1].Walls = new List<WallItem> ();
                    list[list.Count - 1].Texts = new List<TextItem> ();

                    List<TextItem> texts = new List<TextItem> ();

                    int sizeK = isBlockScaling ? 6 : 3;

                    foreach (var text in _texts) {
                        texts.Add (new TextItem () {
                            Id = text.MapID,
                                Key = list[list.Count - 1].Key +
                                "_text_" + text.MapID,
                                X = (int) text.X * 10 * sizeK,
                                Y = (int) text.Y * 10 * sizeK,
                                Text = text.сLevel,
                                FontSize = (int) text.Frame == 0 ? (8 * sizeK) : ((int) text.Frame * sizeK)
                        });
                    }

                    list[list.Count - 1].Texts = texts;
                }
            }
            
            // TODO: adding nabiv sections to others
            foreach (List<StillageItem> nabivGroup in nabivStillagesGroups)
            {
                int minCoordIndex = 0; int min;
                int maxCoordIndex = 0; int max;
                int k = 0;
                if (nabivGroup[0].Orientation == Orientation.HORIZONTAL.ToString())
                {
                    min = nabivGroup[0].Y;
                    max = nabivGroup[0].Y;
                    foreach (StillageItem stItem in nabivGroup)
                    {
                        if (stItem.Y < min)
                        {
                            minCoordIndex = k;
                            min = stItem.Y;
                        }
                        else if (stItem.Y > max)
                        {
                            maxCoordIndex = k;
                            max = stItem.Y;
                        }      
                        k++;
                    }
                }
                else if (nabivGroup[0].Orientation == Orientation.VERTICAL.ToString())
                {
                    min = nabivGroup[0].X;
                    max = nabivGroup[0].X;
                    foreach (StillageItem stItem in nabivGroup)
                    {
                        if (stItem.X < min)
                        {
                            minCoordIndex = k;
                            min = stItem.X;
                        }
                        else if (stItem.X > max)
                        {
                            maxCoordIndex = k;
                            max = stItem.X;
                        }      
                        k++;
                    }
                }

                if (nabivGroup[0].Orientation == Orientation.HORIZONTAL.ToString())
                {
                    nabivGroup[minCoordIndex].Signature = new SignatureItem () {
                        StillageID = nabivGroup[minCoordIndex].Id,
                        Title = nabivGroup[minCoordIndex].Row,
                        Position = SignaturePosition.TOP.ToString()
                    };
                    nabivGroup[maxCoordIndex].Signature = new SignatureItem () {
                        StillageID = nabivGroup[maxCoordIndex].Id,
                        Title = nabivGroup[maxCoordIndex].Row,
                        Position = SignaturePosition.BOTTOM.ToString()
                    };
                }
                else
                {
                    nabivGroup[minCoordIndex].Signature = new SignatureItem () {
                        StillageID = nabivGroup[minCoordIndex].Id,
                        Title = nabivGroup[minCoordIndex].Row,
                        Position = SignaturePosition.LEFT.ToString()
                    };
                    nabivGroup[maxCoordIndex].Signature = new SignatureItem () {
                        StillageID = nabivGroup[maxCoordIndex].Id,
                        Title = nabivGroup[maxCoordIndex].Row,
                        Position = SignaturePosition.RIGHT.ToString()
                    };
                }
            }
            
            const int stillageCellSize = 30;
            int stillageLayerNum = 0;
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Type == LayerType.STILLAGES.ToString ()) {
                    stillageLayerNum = i;
                }
            }

            foreach (List<StillageItem> nabivGroup in nabivStillagesGroups)
            {
                foreach (StillageItem stItem in nabivGroup)
                {
                    stItem.Deviations = new List<DeviationItem>();
                    stItem.PlaceSignatures = new List<PlaceSignatureItem>();
                    stItem.Viks = new List<VikItem>();
                    list[stillageLayerNum].Stillages.Add(stItem);
                }
            }

            // list[stillageLayerNum].Stillages

            try {
                if (list[stillageLayerNum].Stillages.Count != 0) {
                    foreach (var stillage in list[stillageLayerNum].Stillages) {
                        decimal width = 0;
                        decimal height = 0;
                        if (stillage.Scale > 1) {
                            width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                stillage.PmCount * stillageCellSize * stillage.Scale :
                                stillageCellSize * stillage.Scale * 2;
                            height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                stillage.PmCount * stillageCellSize * stillage.Scale :
                                stillageCellSize * stillage.Scale * 2;
                        } else if (stillage.Scale == 1 && stillage.IsBlockScaling) {
                            width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                stillage.PmCount * stillageCellSize * stillage.Scale * 2 :
                                stillageCellSize * stillage.Scale * 2;
                            height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                stillage.PmCount * stillageCellSize * stillage.Scale * 2 :
                                stillageCellSize * stillage.Scale * 2;
                        } else if (!stillage.IsBlockScaling) {
                            width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                stillage.PmCount * stillageCellSize :
                                stillageCellSize;
                            height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                stillage.PmCount * stillageCellSize :
                                stillageCellSize;
                        }

                        if (stillage.Orientation == Orientation.VERTICAL.ToString ()) {
                            int x0 = stillage.X + Convert.ToInt32 (width);
                            int y0 = stillage.Y + Convert.ToInt32 (height);
                            int bad = -1;
                            list[stillageLayerNum].Stillages.ForEach ((el) => {
                                // && (el.Y > y0 && el.Y < y0)
                                if (Math.Abs (el.X - x0) < width - 15 && (el.Y >= stillage.Y && el.Y <= y0)) {
                                    bad = el.Id;
                                }
                            });

                            if (bad != -1) {
                                // Console.WriteLine(true);
                                list[stillageLayerNum]
                                    .Stillages.FirstOrDefault (el => el.Id == stillage.Id).Signature
                                    .Position = SignaturePosition.LEFT.ToString ();
                            }
                        } else {
                            int x0 = stillage.X + Convert.ToInt32 (width);
                            int y0 = stillage.Y + Convert.ToInt32 (height);
                            int bad = -1;
                            list[stillageLayerNum].Stillages.ForEach ((el) => {
                                // && (el.Y > y0 && el.Y < y0)
                                if (Math.Abs (el.Y - y0) < height - 15 && (el.X >= stillage.X && el.X <= x0)) {
                                    bad = el.Id;
                                }
                            });

                            if (bad != -1) {
                                // Console.WriteLine(true);
                                list[stillageLayerNum]
                                    .Stillages.FirstOrDefault (el => el.Id == stillage.Id).Signature
                                    .Position = SignaturePosition.TOP.ToString ();
                            }
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine ("stillage signature forwarding checking fault!");
            }
            Console.WriteLine(nabivStillagesGroups);
            return await Task.Run (() => list);
        }

        // Получение и преобразование данных из формата A100 в формат A100ME
        public async Task<List<Map>> GetMap (int ResoultID) {
            List<Map> map = new List<Map> ();
            v_GetControl control = await MyDB.db.v_GetControl.FirstOrDefaultAsync (i => i.ResoultID == ResoultID);
            List<v_GetUnitNames> _blocks = await MyDB.db.v_GetUnitNames.Where (i => i.ProjectNumber == control.ProjectNumber).ToListAsync ();
            List<A100_MapEngine_DefectInfo> vikInfos = await
            MyDB.db.A100_MapEngine_DefectInfo.Where (el => el.ResoultID == ResoultID).ToListAsync ();
            List<v_GetVikByUnit> viks = await MyDB.db.v_GetVikByUnit.Where (vEl => vEl.ResoultID == ResoultID).ToListAsync ();
            List<Deviation> deviations = await MyDB.db.Deviation.Where (el => el.ResoultId == ResoultID && (el.ShowMode == 0 || el.ShowMode == null)).ToListAsync ();

            int k = 0;
            foreach (v_GetUnitNames block in _blocks) {
                int layerK = 0;

                List<v_GetMap> _stillages = await MyDB.db.v_GetMap.Where (i => (i.ResoultID == ResoultID) && (i.MapUnit == block.MapUnit) && (i.cIndex >= 0)).ToListAsync ();
                List<v_GetMap> _walls = await MyDB.db.v_GetMap.Where (i =>
                    (i.ResoultID == ResoultID) &&
                    (i.MapUnit == block.MapUnit) &&
                    (WallsIndexes.Contains ((int) i.cIndex))).ToListAsync ();
                List<v_GetMap> _texts = await MyDB.db.v_GetMap.Where (i => (i.ResoultID == ResoultID) && (i.MapUnit == block.MapUnit) && (i.cIndex == -2)).ToListAsync ();

                if (_stillages.Count != 0 || _walls.Count != 0 || _texts.Count != 0) {
                    map.Add (new Map () {
                        Id = k,
                            Key = "unit_" + k,
                            Title = block.MapUnit,
                    });
                }

                if (_stillages.Count != 0) {
                    map[map.Count - 1].Layers = new List<MapLayer> ();
                    map[map.Count - 1].Layers.Add (new MapLayer () {
                        Id = layerK,
                            Key = map[map.Count - 1].Key + "_layer_" + layerK,
                            Title = RussianLayerNames.STILLAGES.ToString (),
                            Type = LayerType.STILLAGES.ToString (),
                            MapIconsType = MapIconsType.DRAWING,
                    });
                    map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages = new List<StillageItem> ();
                    map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Objects = new List<ObjectItem> ();
                    map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Walls = new List<WallItem> ();
                    map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Texts = new List<TextItem> ();

                    layerK++;
                    int sK = 0;
                    bool isBlockScaling = false;
                    foreach (v_GetMap el in _stillages) {
                        string _orientation = "";
                        int _x = 0;
                        int _y = 0;
                        decimal pmCount = Math.Floor ((decimal) WidthOrHeight (el.Width, el.Height));
                        int scale = el.Width < el.Height ? (int) el.Width : (int) el.Height;
                        int iteration = 0;
                        Int32.TryParse (el.Iteration.ToString (), out iteration);

                        foreach (var st in _stillages) {
                            int _scale = st.Width < st.Height ? (int) st.Width : (int) st.Height;
                            if (_scale > 1) isBlockScaling = true;
                        }

                        if (el.X != null) _x = ((int) el.X * 10);
                        if (el.Y != null) _y = ((int) el.Y * 10);

                        if (el.Width > el.Height) _orientation = Orientation.HORIZONTAL.ToString ();
                        else if (el.Height > el.Width) _orientation = Orientation.VERTICAL.ToString ();
                        else if (el.Width == el.Height) _orientation = Orientation.HORIZONTAL.ToString ();

                        if (scale == 1) {
                            if (_orientation == Orientation.VERTICAL.ToString ()) {
                                _x *= isBlockScaling ? 6 : 3;
                                _y *= isBlockScaling ? 6 : 3;
                            } else if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                                _x *= isBlockScaling ? 6 : 3;
                                _y *= isBlockScaling ? 6 : 3;
                            }
                        } else {
                            if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                                _x *= 6;
                                _y *= 6;
                            } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                                _x *= 6;
                                // _x += (Convert.ToInt32(el.Width) * 30);
                                _y *= 6;
                            }
                        }

                        List<Deviation> stillageDeviations = deviations.Where (
                            dEl => dEl.Row == el.Row &&
                            dEl.Frame == el.Frame &&
                            dEl.SpecificationsId == el.SpecificationsID
                        ).ToList ();

                        /*
                        List<Deviation> stillageDeviations = new List<Deviation>();
                        if (_orientation == Orientation.HORIZONTAL.ToString())
                        {
                            stillageDeviations = deviations.Where(dEl =>
                                ((dEl.MX >= el.X &&
                                  dEl.MX <= (el.X + 30 * pmCount)) && (dEl.MY == el.Y)) &&
                                (dEl.SpecificationsId == el.SpecificationsID)
                            ).ToList();
                        } else if (_orientation == Orientation.VERTICAL.ToString())
                        {
                            stillageDeviations = deviations.Where(dEl =>
                                ((dEl.MY >= el.Y &&
                                  dEl.MY <= (el.Y + 30 * pmCount)) && (dEl.MX == el.X)) &&
                                (dEl.SpecificationsId == el.SpecificationsID)
                            ).ToList();
                        }
                        */

                        List<DeviationItem> validDeviations = new List<DeviationItem> ();
                        foreach (var dEl in stillageDeviations) {
                            var arrowDirection = dEl.ArrowDirection;
                            var deviationLocation = dEl.DeviationLocationId;
                            validDeviations.Add (new DeviationItem () {
                                Id = dEl.DeviationId,
                                    Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key + "_stillage_" + el.MapID + "_deviation_" + dEl.DeviationId,
                                    StillageID = el.MapID
                            });
                            if (_orientation == Orientation.HORIZONTAL.ToString ()) {
                                if (deviationLocation == 1) {
                                    validDeviations[validDeviations.Count - 1].DeviationPosition =
                                        SignaturePosition.RIGHT.ToString ();
                                } else if (deviationLocation == 2) {
                                    validDeviations[validDeviations.Count - 1].DeviationPosition =
                                        SignaturePosition.LEFT.ToString ();
                                }
                            } else if (_orientation == Orientation.VERTICAL.ToString ()) {
                                if (deviationLocation == 1) {
                                    validDeviations[validDeviations.Count - 1].DeviationPosition =
                                        SignaturePosition.BOTTOM.ToString ();
                                } else if (deviationLocation == 2) {
                                    validDeviations[validDeviations.Count - 1].DeviationPosition =
                                        SignaturePosition.TOP.ToString ();
                                }
                            }

                            if (arrowDirection == 4 || arrowDirection == 1) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = false;
                            } else if (arrowDirection == 2 || arrowDirection == 3) {
                                validDeviations[validDeviations.Count - 1].ArrowFirstToSecond = true;
                            }
                        }

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Add (new StillageItem () {
                            Id = el.MapID,
                                Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key + "_stillage_" + el.MapID,
                                X = _x,
                                Y = _y,
                                Size = StillageSize.NORMAL.ToString (),
                                Orientation = _orientation,
                                PmCount = pmCount,
                                Scale = scale,
                                IsBlockScaling = isBlockScaling,
                                Deviations = validDeviations
                        });
                        // 
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages[map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1].Viks = new List<VikItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages[map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1].PlaceSignatures = new List<PlaceSignatureItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages[map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1].Signature = new SignatureItem ();

                        var pSignatures = new List<PlaceSignatureItem> ();

                        if (Math.Abs (iteration) < 100) {
                            // int iterK = 0;
                            // if (iteration != 0) iterK = Math.Abs(iteration % 100);
                            if (scale != 0) {
                                for (int i = 0; i < pmCount / scale; i++) {
                                    // ?????
                                    int frame = 0;
                                    Int32.TryParse (el.Frame.ToString (), out frame);

                                    var temp = iteration > 0 ?
                                        (frame + Math.Abs (iteration) * i).ToString () :
                                        (frame - Math.Abs (iteration) * i).ToString ();
                                    pSignatures.Add (
                                        new PlaceSignatureItem () {
                                            Place = i + 1,
                                                Title = temp,
                                                StillageID = el.MapID,
                                        }
                                    );
                                }
                            }
                        } else {
                            if (scale != 0) {
                                for (int i = 0; i < pmCount / scale; i++) {
                                    // ?????
                                    int frame = 0;
                                    Int32.TryParse (el.Frame.ToString (), out frame);

                                    pSignatures.Add (
                                        new PlaceSignatureItem () {
                                            Place = i + 1,
                                                Title = frame.ToString (),
                                                StillageID = el.MapID,
                                        }
                                    );
                                }
                            }
                        }

                        var signature = new SignatureItem () {
                            StillageID = el.MapID,
                            Title = el.Row,
                            Position = _orientation == Orientation.HORIZONTAL.ToString () ? SignaturePosition.BOTTOM.ToString () : SignaturePosition.RIGHT.ToString ()
                        };

                        // Повреждения
                        List<v_GetVikByUnit> stillageViks = viks.Where (stVikEl =>
                            stVikEl.Row == el.Row &&
                            stVikEl.Frame == el.Frame &&
                            stVikEl.SpecificationsID == el.SpecificationsID
                            // stVikEl.States == 0 &&
                            // stVikEl.ShowMode == 0
                            // stVikEl.PartialTOID == null
                        ).ToList ();

                        List<VikItem> validDefects = new List<VikItem> ();

                        string projectPhotoPath = @"C:\inetpub\wwwroot\asti\Content\" + ResoultID + @"\VIK\";

                        for (int iter = 0; iter < stillageViks.Count; iter++) {
                            string color = "";
                            int place = 0;

                            if (_orientation == Orientation.VERTICAL.ToString ())
                                place = Math.Abs ((int) el.Y - (int) stillageViks[iter].mY);
                            else if (_orientation == Orientation.HORIZONTAL.ToString ())
                                place = Math.Abs ((int) el.X - (int) stillageViks[iter].mX);

                            if (stillageViks[iter].RiskLevelID == 1) color = "#06F107";
                            else if (stillageViks[iter].RiskLevelID == 2) color = "#FFBB00";
                            else if (stillageViks[iter].RiskLevelID == 3) color = "#FF003C";

                            var vikInfo = vikInfos.FirstOrDefault (pred => pred.VikID == stillageViks[iter].VikID);

                            string photoPath = projectPhotoPath + stillageViks[iter].VikID;
                            List<byte[]> photos = new List<byte[]> ();
                            for (int photoIter = 0; photoIter < 4; photoIter++) {
                                var _path = photoPath + '_' + (photoIter + 1) + ".jpg";
                                if (File.Exists (_path)) {
                                    photos.Add (File.ReadAllBytes (_path));
                                }
                            }
                            if (File.Exists (photoPath + ".jpg")) {
                                photos.Add (File.ReadAllBytes (photoPath + ".jpg"));
                            }

                            photoPath = projectPhotoPath + "R_" + stillageViks[iter].VikID;
                            List<byte[]> repairsPhotos = new List<byte[]> ();
                            for (int rPhotoIter = 0; rPhotoIter < 4; rPhotoIter++) {
                                var _path = photoPath + '_' + (rPhotoIter + 1) + ".jpg";
                                if (File.Exists (_path)) {
                                    repairsPhotos.Add (File.ReadAllBytes (_path));
                                }
                            }
                            if (File.Exists (photoPath + ".jpg")) {
                                repairsPhotos.Add (File.ReadAllBytes (photoPath + ".jpg"));
                            }

                            validDefects.Add (new VikItem () {
                                Id = stillageViks[iter].VikID,
                                    Place = place == 0 ? 1 : place + 1,
                                    Row = vikInfo.Row,
                                    ElementName = vikInfo.ElementName,
                                    Level = Convert.ToInt32 (stillageViks[iter].nLevel),
                                    ElementSize = vikInfo.ElementSize,
                                    Color = color,
                                    ElementManufacturer = vikInfo.ManufacturedStillage,
                                    DefectType = vikInfo.DefectName,
                                    Comment = vikInfo.cComment,
                                    IsRepaired = vikInfo.States == 1,
                                    DefectDate = vikInfo.UpdateTime.ToString (),
                                    RepairDate = vikInfo.RepairDate.ToString (),
                                    DetailsCount = 0,
                                    DefectPhotos = photos,
                                    RepairsPhotos = repairsPhotos,
                                    StillageID = el.MapID
                            });
                        }

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1]
                            .Stillages[
                                map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1]
                            .PlaceSignatures = pSignatures;

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1]
                            .Stillages[
                                map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1]
                            .Viks = validDefects;

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1]
                            .Stillages[
                                map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages.Count - 1]
                            .Signature = signature;

                        sK++;
                    }

                    if (_walls.Count != 0) {
                        map[map.Count - 1].Layers.Add (new MapLayer () {
                            Id = layerK,
                                Key = map[map.Count - 1].Key + "_layer_" + layerK,
                                Title = RussianLayerNames.WALLS.ToString (),
                                Type = LayerType.WALLS.ToString (),
                                MapIconsType = MapIconsType.DRAWING,
                        });
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages = new List<StillageItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Objects = new List<ObjectItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Walls = new List<WallItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Texts = new List<TextItem> ();

                        List<WallItem> walls = new List<WallItem> ();

                        int sizeK = isBlockScaling ? 6 : 3;

                        foreach (v_GetMap el in _walls) {
                            int _startX;
                            int _startY;

                            if (el.cIndex is - 4) { // горизонтальная стена
                                _startX = (int) el.X * 10;
                                _startY = (int) el.Y * 10;
                                if (el.Width < 0) {
                                    _startX = (int) el.X * 10 - Math.Abs ((int) el.Width * 10);
                                }

                                walls.Add (new WallItem () {
                                    Id = el.MapID,
                                        Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key +
                                        "_wall_" + el.MapID,
                                        StartX = _startX * sizeK,
                                        StartY = _startY * sizeK,
                                        Length = Math.Abs ((int) el.Width) * 10 * sizeK,
                                        Orientation = Orientation.HORIZONTAL.ToString (),
                                        Color = ""
                                });
                            } else if (el.cIndex is - 3) { // вертикальная стена
                                _startX = (int) el.X * 10;
                                _startY = (int) el.Y * 10;
                                if (el.Width < 0) {
                                    _startY = (int) el.Y * 10 - Math.Abs ((int) el.Width * 10);
                                }

                                walls.Add (new WallItem () {
                                    Id = el.MapID,
                                        Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key +
                                        "_wall_" + el.MapID,
                                        StartX = _startX * sizeK + 7,
                                        StartY = _startY * sizeK,
                                        Length = Math.Abs ((int) el.Width) * 10 * sizeK,
                                        Orientation = Orientation.VERTICAL.ToString (),
                                        Color = ""
                                });
                            } else if (el.cIndex is - 1) { // колонны
                                walls.Add (new WallItem () {
                                    Id = el.MapID,
                                        Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key +
                                        "_wall_" + el.MapID,
                                        StartX = (int) el.X * 10 * sizeK + 30,
                                        StartY = (int) el.Y * 10 * sizeK + 30,
                                        Length = 10,
                                        Orientation = Orientation.HORIZONTAL.ToString (),
                                        Color = "black"
                                });
                            }
                        }

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Walls = walls;
                    }

                    layerK++;
                    if (_texts.Count != 0) {
                        map[map.Count - 1].Layers.Add (new MapLayer () {
                            Id = layerK,
                                Key = map[map.Count - 1].Key + "_layer_" + layerK,
                                Title = RussianLayerNames.TEXT.ToString (),
                                Type = LayerType.TEXT.ToString (),
                                MapIconsType = MapIconsType.DRAWING,
                        });

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Stillages = new List<StillageItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Objects = new List<ObjectItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Walls = new List<WallItem> ();
                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Texts = new List<TextItem> ();

                        List<TextItem> texts = new List<TextItem> ();

                        int sizeK = isBlockScaling ? 6 : 3;

                        foreach (var text in _texts) {
                            texts.Add (new TextItem () {
                                Id = text.MapID,
                                    Key = map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Key +
                                    "_text_" + text.MapID,
                                    X = (int) text.X * 10 * sizeK,
                                    Y = (int) text.Y * 10 * sizeK,
                                    Text = text.сLevel,
                                    FontSize = (int) text.Frame == 0 ? (8 * sizeK) : ((int) text.Frame * sizeK)
                            });
                        }

                        map[map.Count - 1].Layers[map[map.Count - 1].Layers.Count - 1].Texts = texts;
                    }
                }

                k++;
            }
            // removing empty blocks
            List<int> nullableUnits = new List<int> ();
            for (int h = 0; h < map.Count; h++) {
                if (map[h].Layers == null) {
                    nullableUnits.Add (h);
                }
            }
            foreach (int ind in nullableUnits) {
                map.RemoveRange (ind, 1);
            }

            // проверка рядов 
            foreach (var mapUnit in map) {
                try {
                    if (mapUnit != null) {
                        const int stillageCellSize = 30;
                        var stillageLayer =
                            mapUnit.Layers.FirstOrDefault (el => el.Type == LayerType.STILLAGES.ToString ());
                        foreach (var stillage in stillageLayer.Stillages) {
                            decimal width = 0;
                            decimal height = 0;
                            if (stillage.Scale > 1) {
                                width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                    stillage.PmCount * stillageCellSize * stillage.Scale :
                                    stillageCellSize * stillage.Scale * 2;
                                height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                    stillage.PmCount * stillageCellSize * stillage.Scale :
                                    stillageCellSize * stillage.Scale * 2;
                            } else if (stillage.Scale == 1 && stillage.IsBlockScaling) {
                                width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                    stillage.PmCount * stillageCellSize * stillage.Scale * 2 :
                                    stillageCellSize * stillage.Scale * 2;
                                height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                    stillage.PmCount * stillageCellSize * stillage.Scale * 2 :
                                    stillageCellSize * stillage.Scale * 2;
                            } else if (!stillage.IsBlockScaling) {
                                width = stillage.Orientation == Orientation.HORIZONTAL.ToString () ?
                                    stillage.PmCount * stillageCellSize :
                                    stillageCellSize;
                                height = stillage.Orientation == Orientation.VERTICAL.ToString () ?
                                    stillage.PmCount * stillageCellSize :
                                    stillageCellSize;
                            }

                            if (stillage.Orientation == Orientation.VERTICAL.ToString ()) {
                                int x0 = stillage.X + Convert.ToInt32 (width);
                                int y0 = stillage.Y + Convert.ToInt32 (height);
                                int bad = -1;
                                stillageLayer.Stillages.ForEach ((el) => {
                                    // && (el.Y > y0 && el.Y < y0)
                                    if (Math.Abs (el.X - x0) < width - 15 && (el.Y >= stillage.Y && el.Y <= y0)) {
                                        bad = el.Id;
                                    }
                                });

                                if (bad != -1) {
                                    // Console.WriteLine(true);
                                    mapUnit.Layers
                                        .FirstOrDefault (el => el.Type == LayerType.STILLAGES.ToString ())
                                        .Stillages.FirstOrDefault (el => el.Id == stillage.Id).Signature
                                        .Position = SignaturePosition.LEFT.ToString ();
                                }
                            } else {
                                int x0 = stillage.X + Convert.ToInt32 (width);
                                int y0 = stillage.Y + Convert.ToInt32 (height);
                                int bad = -1;
                                stillageLayer.Stillages.ForEach ((el) => {
                                    // && (el.Y > y0 && el.Y < y0)
                                    if (Math.Abs (el.Y - y0) < height - 15 && (el.X >= stillage.X && el.X <= x0)) {
                                        bad = el.Id;
                                    }
                                });

                                if (bad != -1) {
                                    // Console.WriteLine(true);
                                    mapUnit.Layers
                                        .FirstOrDefault (el => el.Type == LayerType.STILLAGES.ToString ())
                                        .Stillages.FirstOrDefault (el => el.Id == stillage.Id).Signature
                                        .Position = SignaturePosition.TOP.ToString ();
                                }
                            }
                        }
                    }
                } catch (Exception e) { }
            }

            return await Task.Run (() => map);
        }

        public async Task<List<v_GetVik>> GetDefect (int ResoultID, string UnitName, List<int> StillagesID) {
            List<v_GetVik> defects = new List<v_GetVik> ();

            foreach (var stillageId in StillagesID) {
                v_GetMap stillage = await MyDB.db.v_GetMap.FirstOrDefaultAsync (el => el.MapID == stillageId);
            }

            /*
            List<SpecificationsElement> specEls = MyDB.db.SpecificationsElement
                .Where(specEl => specEl.SpecificationsId == el.SpecificationsID && specEl.ResoultId == ResoultID).ToList();
                        
            List<v_GetVik> viks = new List<v_GetVik>();  
            // MyDB.db.v_GetVik.Where(vikEl => vikEl.)
            foreach (var specEl in specEls)
            {
                var _v = MyDB.db.v_GetVik.Where(vikEl =>
                    vikEl.SpecificationsElementID == specEl.SpecificationsElementId && vikEl.mX == el.X &&
                    vikEl.mY == el.Y).ToList();
                viks.AddRange(_v);
            }
            */

            return await Task.Run (() => defects);
        }

        public Task<List<string>> GetUnits (int ResoultID) {
            throw new NotImplementedException ();
        }

        public int GetLayerIndexByType (List<MapLayer> layers, string type) {
            for (int i = 0; i < layers.Count; i++) {
                if (layers[i].Type == LayerType.STILLAGES.ToString ()) return i;
            }
            return -1;
        }

        public decimal? WidthOrHeight (decimal? width, decimal? height) {
            return width > height ? width : height;
        }

    }
}