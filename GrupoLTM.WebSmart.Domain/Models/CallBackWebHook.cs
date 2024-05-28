using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class CallBackWebHook
    {
        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("data")]
        public List<ItemCallBackWebHook> Data { get; set; }

        [JsonProperty("clientInfo")]
        public ClientInfo ClientInfo { get; set; }
    }
    public class ClientInfo
    {
        public int customerId { get; set; }
        public int subAccountId { get; set; }
        public int userId { get; set; }
    }

    public partial class ItemCallBackWebHook
    {
        public int id_ltm { get; set; }
        public string id { get; set; }
        public string correlationId { get; set; }
        public string destination { get; set; }
        public int deliveredStatusCode { get; set; }
        public string deliveredStatus { get; set; }
        public string deliveredDate { get; set; }

        private DateTime? _myVal = DateTime.Now;
        public DateTime? datainclusao { get { return _myVal; } set { _myVal = value; } }
    }
}
