using System.Text.Json.Serialization;

namespace HackerNews.Data.Entity
{
    public class HackerNewsItem
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("by")]
        public string? By { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }   // Unix time (seconds)

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("descendants")]
        public int Descendants { get; set; }  // Comment count

        [JsonPropertyName("kids")]
        public List<long>? Kids { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("parent")]
        public long? Parent { get; set; }

        [JsonPropertyName("parts")]
        public List<long>? Parts { get; set; }

        [JsonPropertyName("dead")]
        public bool? Dead { get; set; }

        [JsonPropertyName("deleted")]
        public bool? Deleted { get; set; }
    }
}
