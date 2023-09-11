using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JHReddit.DTO
{
    public class CredentialsDto
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("AppSecrete")]
        public string AppSecrete { get; set; }

    }
}
