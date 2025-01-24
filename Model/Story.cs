using System.Text.Json.Serialization;

public class Story
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Uri { get; set; }

    [JsonPropertyName("by")]
    public string PostedBy { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("descendants")]
    public int CommentCount { get; set; }
}
