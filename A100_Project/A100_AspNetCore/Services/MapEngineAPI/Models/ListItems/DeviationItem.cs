namespace A100_AspNetCore.Services.MapEngineAPI.Models.ListItems
{
    public class DeviationItem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string DeviationPosition { get; set; }
        public bool ArrowFirstToSecond { get; set; }
        public int StillageID { get; set; }
    }
}