using Newtonsoft.Json;
using System;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class OrigemLive
    {
        public OrigemLive()
        {
        }

        public OrigemLive(string name, string clientId, string campaignId, string billingCode)
        {
            StatusId = 1;
            SendToProtheus = 1;
            ClientId = clientId;
            CampaignId = campaignId;
            BillingCode = billingCode;
            Name = name;
            FriendlyName = name;
        }

        [JsonProperty("originId")]
        public int OriginId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("transactionsExpirationDate")]
        public DateTime? TransactionsExpirationDate { get; set; }

        [JsonProperty("statusId")]
        public int StatusId { get; set; }

        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        [JsonProperty("transactionsExpirationPeriod")]
        public string TransactionsExpirationPeriod { get; set; }

        [JsonProperty("dateIntervalId")]
        public int? DateIntervalId { get; set; }

        [JsonProperty("campaignId")]
        public string CampaignId { get; set; }

        [JsonProperty("billingCode")]
        public string BillingCode { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("sendToProtheus")]
        public int SendToProtheus { get; set; }
    }
}
