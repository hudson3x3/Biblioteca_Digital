using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ArquivoClubeEstrelasRegister
    {
        public int Id { get; set; }

        public int ArquivoId { get; set; }

        public string RegisterType { get; set; }

        public int? RepresentativeAccountNumber { get; set; }

        public string RepresentativeClassification { get; set; }

        public int? CampaignNumber { get; set; }

        public int? CampaignYear { get; set; }

        public string ProgramName { get; set; }

        public long? TotalPaymentAmount { get; set; }

        public long? TotalCanceledPoints { get; set; }

        public long? TotalPendingPoints { get; set; }

        public long? TotalAvailablePoints { get; set; }

        public int? MMACalculationCampaignNumber { get; set; }

        public int? MMACalculationCampaignYear { get; set; }

        public string MMAGoalMeet { get; set; }

        public int? MMAPercentageBonus { get; set; }

        public int? OrderID { get; set; }

        public string PointsTagForTransaction { get; set; }

        public string Description { get; set; }

        public int? Factor { get; set; }

        public long? PaymentAmount { get; set; }

        public long? CanceledPoints { get; set; }

        public long? PendingPoints { get; set; }

        public long? AvailablePoints { get; set; }

        public DateTime? TransactionCreationDate { get; set; }

        public DateTime? PointsExpirationDate { get; set; }

        public int LoteId { get; set; }

        public int StatusId { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public string OrderIDCancelledTag { get; set; }

        public int RebillingOrderID { get; set; }

        public int? RebillingCampaignNumber { get; set; }

        public int? RebillingCampaignYear { get; set; }

        public DateTime? PointsDueDate { get; set; }

    }
}
