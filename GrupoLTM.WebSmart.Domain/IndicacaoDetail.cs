using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class IndicacaoDetail
    {
        public int Id { get; set; }
        public string RecordType { get; set; }
        public string ReferredRepresentativeNumberDetail { get; set; }
        public string ReferredNameDetail { get; set; }
        public int ReferenceCampaignNumberDetail { get; set; }
        public int ReferenceCampaignYearNumberDetail { get; set; }
        public int ReferredAppointmentCampaignNumberDetail { get; set; }
        public int ReferredAppointmentCampaignYearNumberDetail { get; set; }
        public int OrderCampaignNumberDetail { get; set; }
        public int OrderCampaignYearNumberDetail { get; set; }
        public string SentOrderCodeDetail { get; set; }
        public string OrderPaymentCodeDetail { get; set; }
        public string ReturnedOrderCodeDetail { get; set; }
        public string ReferralRepresentativeNumberDetail { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public int IndicacaoHeaderId { get; set; }
        public virtual IndicacaoHeader IndicacaoHeader { get; set; }
    }
}
