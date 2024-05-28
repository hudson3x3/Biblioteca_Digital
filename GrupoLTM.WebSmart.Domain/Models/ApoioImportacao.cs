using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ApoioImportacao
    {
        ///Detail
        public string ProductIncentiveProgramTypeNumber { get; set; }
        public string IncentiveProgramDescription { get; set; }
        public string AccountNumber { get; set; }
        public string CampaignYear { get; set; }
        public string CampaignNumber { get; set; }
        public string TotalCancelledPointAmount { get; set; }
        public string TotalEstimatedPointAmount { get; set; }
        public string TotalValidPointAmount { get; set; }
        public string TotalSalesAmount { get; set; }
        public string TotalReturnAmount { get; set; }
        public string ProcessDate { get; set; }
        public string ZoneNumber { get; set; }
        public string TeamNumber { get; set; }
        public string PointExpirationDate { get; set; }
        public string ProductIncentiveProgramNumber { get; set; }

        public int LoteId { get; set; }
        public int Id { get; set; }
        public int NumeroLinha { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string Erro { get; set; }
        public virtual Lote Lote { get; set; }
        public string LinhaConteudo { get; set; }
    }
}
