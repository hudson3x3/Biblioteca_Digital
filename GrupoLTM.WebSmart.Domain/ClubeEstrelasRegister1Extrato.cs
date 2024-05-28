using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ClubeEstrelasRegister1Extrato
    {
        public int Id { get; set; }

        public int ArquivoClubeEstrelasRegisterId { get; set; }

        public long MktPlaceCatalogoId { get; set; }

        public string ProgramName { get; set; }

        public int CampaignNumber { get; set; }

        public int CampaignYear { get; set; }

        public int? RepresentativeAccountNumber { get; set; }

        public string RepresentativeClassification { get; set; }

        public long? TotalPaymentAmount { get; set; }

        public long? TotalCanceledPoints { get; set; }

        public long? TotalPendingPoints { get; set; }

        public long? TotalAvailablePoints { get; set; }

        public int? MMACalculationCampaignNumber { get; set; }

        public int? MMACalculationCampaignYear { get; set; }

        public string MMAGoalMeet { get; set; }

        public int? MMAPercentageBonus { get; set; }

        public string FezJusIndicator { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

    }
}
