using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Models.A100_Models.DataBase._Views;
using A100_AspNetCore.Models.A100_Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Services.API.VikService
{
    public interface IVikService
    {
        Task<CameraEvents> UpdateCameraEvent(CameraEvents cEvnt);

        // GET - тестовый метод для проверки деплоинга
        Task<List<v_GetVik>> GetHello(int resoultID);

        // GET - метод, который получает VIK по ResoultID
        Task<List<v_GetVik>> GetVik(int ResoultID);

        Task<DTOAddVik> AddNewVik(Vik vik);
        Task<object> UpdateVik(int VikID, Vik vik);

        // GET - метод, который получает названия VIK
        Task<List<VikElement>> GetVikNames();

        // GET - метод, который получает список фото повреждений, имеющихся на сервере
        Task<List<byte[]>> GetVIKPhotoList(int idVik);

        // POST - метод, который загружает фото повреждения с сервера на устройство
        Task<DTOPhoto> GetVikPhoto(int rid, int idVik);

        Task<List<DTOVikByUnit>> GetViksByUnit(int resoultID, string unitName);

        Task<CameraEvents> GetCameraEvents(string userName);
        Task<string> DeleteCameraEvents(string username);
        Task<string> RegisterCameraEvent(CameraEvents camEvent);
        Task<List<DTOVikByUnit>> GetViksByPartialTO(int partialTOID);
        Task<object> AddVikPhoto(int resoultID, int vikID, byte[] photo);
    }
}
