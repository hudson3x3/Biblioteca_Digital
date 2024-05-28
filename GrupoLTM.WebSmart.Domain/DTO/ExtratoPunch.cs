using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Domain.DTO
{
    public class ExtratoPunch
    {
        public string IncentiveProgram { get; set; }

        public string AccountNumber { get; set; }

        public string StatusPoints { get; set; }

        public decimal Points { get; set; }

        public long? AvailablePoints { get; set; }

        public long? PendingPoints { get; set; }

        public long? CanceledPoints { get; set; }

        public EnumDomain.TipoArquivo TipoArquivo { get; set; }

        public int CampaignNumber { get; set; }

        public int CampaignYear { get; set; }

        public int? EndCampaignNumber { get; set; }

        public int? EndCampaignYear { get; set; }

        public int OrderId { get; set; }
    }
}
