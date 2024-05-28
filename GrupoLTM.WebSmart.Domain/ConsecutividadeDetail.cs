using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConsecutividadeDetail
    {
        public int Id { get; set; }
        public string ProgramDescriptionDetail { get; set; }
        public string RepresentativeNumberDetail { get; set; }
        public int CampaignYearDetail { get; set; }
        public int CampaignNumberDetail { get; set; }
        public string OrderSentCodeDetail { get; set; }
        public string OrderPaymentStatusDetail { get; set; }
        public string OrderObjectiveStatusDetail { get; set; }
        public string OrderDevolutionStatusDetail { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao{ get; set; }
        public bool Ativo { get; set; }
        public int ConsecutividadeHeaderId { get; set; }
        public virtual ConsecutividadeHeader ConsecutividadeHeader { get; set; }
        public string LinhaConteudo { get; set; }
    }
}
