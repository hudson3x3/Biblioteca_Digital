namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesBulk_Indice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConsecutividadeHeader", "RecordTypeHeader", c => c.String(maxLength: 1));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentativeNumberHeader", c => c.String(maxLength: 5));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentiveAscNumberHeader", c => c.String(maxLength: 3));
            AlterColumn("dbo.ConsecutividadeDetail", "RecordTypeDetail", c => c.String(maxLength: 1));
            AlterColumn("dbo.ApoioImportacao", "ProductIncentiveProgramTypeNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "IncentiveProgramDescription", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "AccountNumber", c => c.String(maxLength: 8));
            AlterColumn("dbo.ApoioImportacao", "CampaignYear", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "CampaignNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TotalCancelledPointAmount", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TotalEstimatedPointAmount", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TotalValidPointAmount", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TotalSalesAmount", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TotalReturnAmount", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "ProcessDate", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "ZoneNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "TeamNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "PointExpirationDate", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApoioImportacao", "ProductIncentiveProgramNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "RecordType", c => c.String(maxLength: 1));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramIdentifierHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramType", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramDescription", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeNumberHeader", c => c.String(maxLength: 5));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentiveAscNumberHeader", c => c.String(maxLength: 3));
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignYearValidation", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignNumberValidation", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "CancelledPoints", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "EstimatedPoints", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProvidedPoints", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProcessingDate", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeAppointmentCampaignYear", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeAppointmentCampaignNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ConsecutiveCampaignQuantity", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "Zone", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "Team", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ExpirationDate", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramIdentifierDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeNumberDetail", c => c.String(maxLength: 5));
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentiveAscNumberDetail", c => c.String(maxLength: 3));
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignYear", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderSentCode", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderPaymentStatus", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderReturnStatus", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "RecordType", c => c.String(maxLength: 1));
            AlterColumn("dbo.IndicacaoImportacao", "IncentiveProgramDescriptionHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "CampaignNumberHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "CampaignYearNumberHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "RegionNumberHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "DivisionCodeHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ZoneNumberHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferralRepresentativeNumberHeader", c => c.String(maxLength: 8));
            AlterColumn("dbo.IndicacaoImportacao", "ReferralNameHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "PendingPaymentHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "RepresentativeStatusHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "PointExpirationDateHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "TotalPointsAmountHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "TransactionDateHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferredRepresentativeNumberHeader", c => c.String(maxLength: 8));
            AlterColumn("dbo.IndicacaoImportacao", "FezJusIndicatorHeader", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferredRepresentativeNumberDetail", c => c.String(maxLength: 8));
            AlterColumn("dbo.IndicacaoImportacao", "ReferredNameDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferenceCampaignNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferenceCampaignYearNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferredAppointmentCampaignNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferredAppointmentCampaignYearNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "OrderCampaignNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "OrderCampaignYearNumberDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "SentOrderCodeDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "OrderPaymentCodeDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReturnedOrderCodeDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.IndicacaoImportacao", "ReferralRepresentativeNumberDetail", c => c.String(maxLength: 8));

            CreateIndex("dbo.IndicacaoHeader",
                    new[] { "LoteId" , "ReferredRepresentativeNumberHeader" , "ReferralRepresentativeNumberHeader" },
                    clustered: false,
                    name: "IDX_IndicacaoHeader");

            CreateIndex("dbo.IndicacaoImportacao",
                    new[] { "RecordType", "LoteId" , "ReferredRepresentativeNumberDetail" , "ReferralRepresentativeNumberDetail" },
                    clustered: false,
                    name: "IDX_IndicacaoImportacao");

            CreateIndex("dbo.ConsecutividadeHeader",
                    new[] { "LoteId", "RepresentativeNumberHeader", "RepresentiveAscNumberHeader" },
                    clustered: false,
                    name: "IDX_ConsecutividadeHeader");

            CreateIndex("dbo.ConsecutividadeImportacao",
                    new[] { "RecordType", "LoteId", "RepresentativeNumberDetail", "RepresentiveAscNumberDetail" },
                    clustered: false,
                    name: "IDX_ConsecutividadeImportacao");

            CreateIndex("dbo.ApoioDetail",
                    new[] { "LoteId" },
                    clustered: false,
                    name: "IDX_ApoioDetail");

            CreateIndex("dbo.ApoioImportacao",
                    new[] { "LoteId" },
                    clustered: false,
                    name: "IDX_ApoioImportacao");

        }
        
        public override void Down()
        {
            AlterColumn("dbo.IndicacaoImportacao", "ReferralRepresentativeNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReturnedOrderCodeDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "OrderPaymentCodeDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "SentOrderCodeDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "OrderCampaignYearNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "OrderCampaignNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferredAppointmentCampaignYearNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferredAppointmentCampaignNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferenceCampaignYearNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferenceCampaignNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferredNameDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferredRepresentativeNumberDetail", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "FezJusIndicatorHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferredRepresentativeNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "TransactionDateHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "TotalPointsAmountHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "PointExpirationDateHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "RepresentativeStatusHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "PendingPaymentHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferralNameHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ReferralRepresentativeNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "ZoneNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "DivisionCodeHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "RegionNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "CampaignYearNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "CampaignNumberHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "IncentiveProgramDescriptionHeader", c => c.String());
            AlterColumn("dbo.IndicacaoImportacao", "RecordType", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderReturnStatus", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderPaymentStatus", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "OrderSentCode", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignNumber", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignYear", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentiveAscNumberDetail", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeNumberDetail", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramIdentifierDetail", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ExpirationDate", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "Team", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "Zone", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ConsecutiveCampaignQuantity", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeAppointmentCampaignNumber", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeAppointmentCampaignYear", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProcessingDate", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProvidedPoints", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "EstimatedPoints", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "CancelledPoints", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignNumberValidation", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "CampaignYearValidation", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentiveAscNumberHeader", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RepresentativeNumberHeader", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramDescription", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramType", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "ProgramIdentifierHeader", c => c.String());
            AlterColumn("dbo.ConsecutividadeImportacao", "RecordType", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "ProductIncentiveProgramNumber", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "PointExpirationDate", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TeamNumber", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "ZoneNumber", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "ProcessDate", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TotalReturnAmount", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TotalSalesAmount", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TotalValidPointAmount", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TotalEstimatedPointAmount", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "TotalCancelledPointAmount", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "CampaignNumber", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "CampaignYear", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "AccountNumber", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "IncentiveProgramDescription", c => c.String());
            AlterColumn("dbo.ApoioImportacao", "ProductIncentiveProgramTypeNumber", c => c.String());
            AlterColumn("dbo.ConsecutividadeDetail", "RecordTypeDetail", c => c.String(maxLength: 100));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentiveAscNumberHeader", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentativeNumberHeader", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeHeader", "RecordTypeHeader", c => c.String(maxLength: 100));
        }
    }
}
