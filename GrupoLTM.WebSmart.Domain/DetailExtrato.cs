using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class DetailExtrato
    {
        public int Id { get; set; }
        public string ReferredRepresentativeNumberDetail { get; set; }
        public int ReferredAppointmentCampaignNumberDetail { get; set; }
        public int ReferredAppointmentCampaignYearNumberDetail { get; set; }
        public string ReferralRepresentativeNumberDetail { get; set; }
        public int ReferenceCampaignNumberDetail { get; set; }
        public int ReferenceCampaignYearNumberDetail { get; set; }
        public string SentOrderCodeDetail { get; set; }
        public string OrderPaymentCodeDetail { get; set; }
        public string ReturnedOrderCodeDetail { get; set; }

        public int IndicacaoHeaderExtratoId { get; set; }
       
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }

        /// <summary>
        /// Indicação - I / Consecutividade - C / Apoio - T / Atividade - A / Produtividade - P / Ajustes - J
        /// </summary>
        public string TypePoints { get; set; }
        public string ReferredNameDetail { get; set; }
        public int OrderCampaignNumberDetail { get; set; }
        
        //APOIO e  ATIVIDADE
        public string ProgramType { get; set; }
        public decimal TotalSalesDetail { get; set; }
        public decimal TotalReturnAmountDetail { get; set; }

        //Clube das estrelas
        public int MktPlaceCatalogoId { get; set; }
        public bool AtingiuTurbo { get; set; }
        public DateTime? TransactionCreationDate { get; set; }
        public long CanceledPoints { get; set; }
        public long PendingPoints { get; set; }
        public long AvailablePoints { get; set; }
        public string OrderId { get; set; }
        public string Descricao { get; set; }
        public bool Bonus { get; set; }
        public string PercentualBonus { get; set; }
    }
}
