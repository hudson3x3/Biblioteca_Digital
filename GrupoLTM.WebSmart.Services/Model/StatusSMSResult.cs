using Newtonsoft.Json;
using System;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class StatusSMSResult
    {
        [JsonProperty("_header")]
        public Header Header { get; set; }

        [JsonProperty("_body")]
        public string bBody { get; set; }
        public string AppId { get; set; }
        public object Request { get; set; }
        public bool Success { get; set; }
        public bool HasError { get; set; }
        public string ErrorDetail { get; set; }
        public object Result { get; set; }        
    }

  
    public class Header
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("content-type")]
        public string ContentType { get; set; }

        [JsonProperty("connection")]
        public string Connection { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }
    }
}

    
