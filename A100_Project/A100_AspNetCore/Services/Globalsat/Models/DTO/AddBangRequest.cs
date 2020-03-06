using System;

namespace A100_AspNetCore.Services.Globalsat.Models.DTO
{
    public class AddBangRequest
    {
        public int SensorId { get; set; }
        public int Strength { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int ResoultId { get; set; }
    }
}