namespace TravelQuotesApi.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}