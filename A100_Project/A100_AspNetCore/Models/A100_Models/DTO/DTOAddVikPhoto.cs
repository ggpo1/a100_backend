using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A100_AspNetCore.Models.A100_Models.DTO
{
    public class DTOAddVikPhoto
    {
        public DTOAddVikPhoto(int resoultID, int vikID, byte[] photo)
        {
            ResoultID = resoultID;
            VikID = vikID;
            Photo = photo;
        }

        public int ResoultID { get; set; }
        public int VikID { get; set; }
        public byte[] Photo { get; set; }
    }
}
