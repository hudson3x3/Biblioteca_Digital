using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class TokenCognito
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpireIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
