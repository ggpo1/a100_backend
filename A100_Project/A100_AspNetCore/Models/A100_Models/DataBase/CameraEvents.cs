using System;
using System.ComponentModel.DataAnnotations;

namespace A100_AspNetCore.Models.A100_Models.DataBase
{
    public partial class CameraEvents
    {
        [Key]
        public int CameraEventId { get; set; }

        public string UserName { get; set; }

        public int vikID { get; set; }

        public byte[] ImageByte { get; set; }

        public int? Done { get; set; }
    }
}
