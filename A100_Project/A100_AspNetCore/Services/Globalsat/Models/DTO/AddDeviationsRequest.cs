using System;

namespace A100_AspNetCore.Services.Globalsat.Models.DTO
{
    public class AddDeviationsRequest
    {
        public string SensorId { get; set; }
        public float? DeviationValue { get; set; }
        public DateTime DeviationDate { get; set; }
    }
}