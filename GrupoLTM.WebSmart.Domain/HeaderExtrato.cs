using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class HeaderExtrato
    {
        public HeaderExtrato()
        {
        }

        //Header Extrato
        public int Id { get; set; }

        public string ProgramDescription { get; set; }

        public int CampaignNumber { get; set; }

        public int CampaignYearNumber { get; set; }

        public int EndCampaignNumber { get; set; }

        public int EndCampaignYearNumber { get; set; }

        public string ReferralRepresentativeNumber { get; set; }

        public string ReferralName { get; set; }

        public string PendingPayment { get; set; }

        public string RepresentativeStatus { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string ReferredRepresentativeNumber { get; set; }

        public string ReferredNameDetail { get; set; }

        public string StatusPoints { get; set; }

        public string OrderId { get; set; }

        public decimal TotalSalesDetail { get; set; }

        public bool AtingiuTurbo { get; set; }

        public string PercentualBonus { get; set; }

        /// <summary>
        /// Indicação - I / Consecutividade - C / Apoio - T / Atividade - A / Produtividade - P / Ajustes - J
        /// </summary>
        public string TypePoints { get; set; }

        /// <summary>
        /// Pontos
        /// </summary>
        public decimal Points { get; set; }

        /// <summary>
        /// Tipo de programa. Especial para apoio e atividade
        /// </summary>
        public int ProgramType { get; set; }
    }
}
