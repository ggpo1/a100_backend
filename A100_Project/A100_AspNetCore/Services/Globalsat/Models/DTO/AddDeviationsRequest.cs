namespace A100_AspNetCore.Services.Globalsat.Models.DTO
{
    public class AddDeviationsRequest
    {
        public int SensorId { get; set; }
        public int DeviationValue { get; set; }
    }
}