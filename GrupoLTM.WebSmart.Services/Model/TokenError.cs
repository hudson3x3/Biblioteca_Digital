using Newtonsoft.Json;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class TokenErrors
    {
        [JsonProperty("errors")]
        public List<Error> errors { get; set; }
    }
    public class Error
    {
        [JsonProperty("code")]
        public int code { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }

}
