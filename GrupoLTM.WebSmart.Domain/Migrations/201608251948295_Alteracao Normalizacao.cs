namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoNormalizacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ConsecutividadeDetail", "StatusId", "dbo.StatusDetail");
            DropForeignKey("dbo.IndicacaoDetail", "StatusId", "dbo.StatusDetail");
            DropIndex("dbo.ConsecutividadeDetail", new[] { "StatusId" });
            DropIndex("dbo.IndicacaoDetail", new[] { "StatusId" });
            AddColumn("dbo.ConsecutividadeHeader", "StatusId", c => c.Int(nullable: false));
            AddColumn("dbo.IndicacaoHeader", "StatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ApoioImportacao", "NumeroLinha", c => c.Int(nullable: false));
            AddColumn("dbo.ConsecutividadeImportacao", "NumeroLinha", c => c.Int(nullable: false));
            AddColumn("dbo.IndicacaoImportacao", "NumeroLinha", c => c.Int(nullable: false));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentativeNumberDetail", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentiveAscNumberDetail", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentativeNumberHeader", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentiveAscNumberHeader", c => c.String(maxLength: 4));
            CreateIndex("dbo.ConsecutividadeHeader", "StatusId");
            CreateIndex("dbo.IndicacaoHeader", "StatusId");
            AddForeignKey("dbo.ConsecutividadeHeader", "StatusId", "dbo.StatusDetail", "Id");
            AddForeignKey("dbo.IndicacaoHeader", "StatusId", "dbo.StatusDetail", "Id");
            DropColumn("dbo.ConsecutividadeDetail", "StatusId");
            DropColumn("dbo.IndicacaoDetail", "StatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IndicacaoDetail", "StatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ConsecutividadeDetail", "StatusId", c => c.Int(nullable: false));
            DropForeignKey("dbo.IndicacaoHeader", "StatusId", "dbo.StatusDetail");
            DropForeignKey("dbo.ConsecutividadeHeader", "StatusId", "dbo.StatusDetail");
            DropIndex("dbo.IndicacaoHeader", new[] { "StatusId" });
            DropIndex("dbo.ConsecutividadeHeader", new[] { "StatusId" });
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentiveAscNumberHeader", c => c.Int(nullable: false));
            AlterColumn("dbo.ConsecutividadeHeader", "RepresentativeNumberHeader", c => c.Int(nullable: false));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentiveAscNumberDetail", c => c.Int(nullable: false));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentativeNumberDetail", c => c.Int(nullable: false));
            DropColumn("dbo.IndicacaoImportacao", "NumeroLinha");
            DropColumn("dbo.ConsecutividadeImportacao", "NumeroLinha");
            DropColumn("dbo.ApoioImportacao", "NumeroLinha");
            DropColumn("dbo.IndicacaoHeader", "StatusId");
            DropColumn("dbo.ConsecutividadeHeader", "StatusId");
            CreateIndex("dbo.IndicacaoDetail", "StatusId");
            CreateIndex("dbo.ConsecutividadeDetail", "StatusId");
            AddForeignKey("dbo.IndicacaoDetail", "StatusId", "dbo.StatusDetail", "Id");
            AddForeignKey("dbo.ConsecutividadeDetail", "StatusId", "dbo.StatusDetail", "Id");
        }
    }
}
