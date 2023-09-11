using Reddit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JHReddit.DTO
{
    public class UserRedditPostDto
    {
        public UserRedditPostDto() { }        

        [JsonPropertyName("Author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("PostCount")]
        public int PostCount { get; set; } = 0;
    }
}
