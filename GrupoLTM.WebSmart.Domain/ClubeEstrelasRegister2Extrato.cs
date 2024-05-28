using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ClubeEstrelasRegister2Extrato
    {
        public int Id { get; set; }

        public int LoteId { get; set; }

        public int ClubeEstrelasRegister1ExtratoId { get; set; }

        public int ArquivoClubeEstrelasRegisterId { get; set; }

        public int? RepresentativeAccountNumber { get; set; }

        public int? CampaignNumber { get; set; }

        public int? CampaignYear { get; set; }

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

        public string FezJusIndicator { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        public int? ProgramaIncentivoId { get; set; }

        public string OrderIDCancelledTag { get; set; }

        public int? RebillingOrderID { get; set; }

        public int? RebillingCampaignNumber { get; set; }

        public int? RebillingCampaignYear { get; set; }

        public DateTime? PointsDueDate { get; set; }
    }
}
