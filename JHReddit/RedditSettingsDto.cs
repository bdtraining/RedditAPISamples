using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JHReddit.DTO
{
    public class RedditSettingsDto
    {
        [JsonPropertyName("SubReddits")]
        public List<string> SubReddits { get; set; } = new();

        [JsonPropertyName("Credentials")]
        public CredentialsDto Credentials { get; set; } = new();

        [JsonPropertyName("MaxTryOnError")]
        public int MaxTryOnError { get; set; } = 3;

        [JsonPropertyName("TimerRefreshInSeconds")]
        public int TimerRefreshInSeconds { get; set; } = 30;
    }
}
