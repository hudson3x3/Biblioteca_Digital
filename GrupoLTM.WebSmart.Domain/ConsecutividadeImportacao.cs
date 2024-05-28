using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ConsecutividadeImportacao
    {
        //Header
        public int Id{ get; set; }
        public string RecordType { get; set; }
        public string ProgramIdentifierHeader { get; set; }
        public string ProgramDescriptionHeader{ get; set; }
        public string RepresentativeNumberHeader { get; set; }
        public string CancelledPointsHeader { get; set; }
        public string EstimatedPointsHeader { get; set; }
        public string ProvidedPointsHeader { get; set; }
        public string ProcessingDateHeader { get; set; }
        public string ExpirationDateHeader { get; set; }
        //Detail
        public string CampaignYearDetail { get; set; }
        public string CampaignNumberDetail { get; set; }
        
        public string OrderSentCodeDetail { get; set; }
        public string OrderPaymentStatusDetail { get; set; }
        public string OrderObjectiveStatusDetail { get; set; }
        public string OrderDevolutionStatusDetail { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Erro { get; set; }
        public virtual Lote Lote { get; set; }
        public string LinhaConteudo { get; set; }
        public int LoteId { get; set; }
    }
}
