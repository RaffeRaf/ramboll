namespace RambollAPI.Models
{
    public class DataResponse
    {
        public int Rows { get; set; }
        public int Os { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public Dictionary<string, DataModel> Documents { get; set; } = new Dictionary<string, DataModel>();
    }
}
