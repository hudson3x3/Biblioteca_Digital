using Newtonsoft.Json;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class ResponseOrigensLive
    {
        [JsonProperty("originId")]
        public int OriginId { get; set; }

        [JsonProperty("origins")]
        public List<OrigemLive> Origins { get; set; }
    }
}
