namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TabelaExtratoIndicacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IndicacaoExtrato",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncentiveProgramDescriptionHeader = c.String(maxLength: 20),
                        CampaignNumberHeader = c.Int(nullable: false),
                        CampaignYearNumberHeader = c.Int(nullable: false),
                        ReferralRepresentativeNumberHeader = c.String(maxLength: 8),
                        ReferralNameHeader = c.String(maxLength: 50),
                        PendingPaymentHeader = c.String(maxLength: 1),
                        RepresentativeStatusHeader = c.String(maxLength: 1),
                        TotalPointsAmountHeader = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionDateHeader = c.DateTime(nullable: false),
                        ReferredRepresentativeNumberHeader = c.String(maxLength: 8),
                        FezJusIndicatorHeader = c.String(maxLength: 1),
                        ReferredRepresentativeNumberDetail = c.String(maxLength: 8),
                        ReferredAppointmentCampaignNumberDetail = c.Int(nullable: false),
                        ReferredAppointmentCampaignYearNumberDetail = c.Int(nullable: false),
                        ReferralRepresentativeNumberDetail = c.String(maxLength: 8),
                        ReferenceCampaignNumberDetail = c.Int(nullable: false),
                        ReferenceCampaignYearNumberDetail = c.Int(nullable: false),
                        SentOrderCodeDetail = c.String(maxLength: 2),
                        OrderPaymentCodeDetail = c.String(maxLength: 2),
                        ReturnedOrderCodeDetail = c.String(maxLength: 2),
                        IndicacaoHeaderId = c.Int(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IndicacaoExtrato");
        }
    }
}
