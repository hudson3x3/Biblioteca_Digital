using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class TokenType
    {
        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Login")]
        public long Login { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("SimuladorVisualizado")]
        public string SimuladorVisualizado { get; set; }

        [JsonProperty("HasInvalidEmail")]
        public bool HasInvalidEmail { get; set; }
    }
}
