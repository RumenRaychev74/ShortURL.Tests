using System.Text.Json.Serialization;

namespace ShortURL.ApiTests
{
    public class CreateUrlResponse
    {

        public string msg { get; set; }
        public UrlResponse url { get; set; }
    }

    public class UrlResponse
    {
        public string url { get; set; }
        public string shortCode { get; set; }
        public string shortUrl { get; set; }
        public string dateCreated { get; set; }
        public int visits { get; set; }

    }
}