using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ApoioDetail
    {
        public int Id { get; set; }
        public int ProductIncentiveProgramTypeNumberDetail { get; set; }
        public string IncentiveProgramDescriptionDetail { get; set; }
        public string AccountNumberDetail { get; set; }
        public int CampaignYearDetail { get; set; }
        public int CampaignNumberDetail { get; set; }
        
        public long Points { get; set; }
        public string StatusPoints { get; set; }

        public decimal TotalSalesDetail { get; set; }
        public decimal TotalReturnAmountDetail { get; set; }
        public string ProcessDateDetail { get; set; }
        public int ZoneNumberDetail { get; set; }
        public int TeamNumberDetail { get; set; }
        public string PointExpirationDateDetail { get; set; }
        public int ProductIncentiveProgramNumberDetail { get; set; }

        public int StatusId { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual StatusDetail StatusDetail { get; set; }
        public int LoteId { get; set; }
        public virtual Lote Lote { get; set; }
    }
}
