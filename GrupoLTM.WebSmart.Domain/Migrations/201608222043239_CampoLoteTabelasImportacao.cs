namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoLoteTabelasImportacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApoioImportacao", "ArquivoId", "dbo.Arquivo");
            DropForeignKey("dbo.ConsecutividadeImportacao", "ArquivoId", "dbo.Arquivo");
            DropForeignKey("dbo.IndicacaoImportacao", "ArquivoId", "dbo.Arquivo");
            DropIndex("dbo.ApoioImportacao", new[] { "ArquivoId" });
            DropIndex("dbo.ConsecutividadeImportacao", new[] { "ArquivoId" });
            DropIndex("dbo.IndicacaoImportacao", new[] { "ArquivoId" });
            AddColumn("dbo.ApoioImportacao", "LoteId", c => c.Int(nullable: false));
            AddColumn("dbo.ConsecutividadeImportacao", "LoteId", c => c.Int(nullable: false));
            AddColumn("dbo.IndicacaoImportacao", "LoteId", c => c.Int(nullable: false));
            CreateIndex("dbo.ApoioImportacao", "LoteId");
            CreateIndex("dbo.ConsecutividadeImportacao", "LoteId");
            CreateIndex("dbo.IndicacaoImportacao", "LoteId");
            AddForeignKey("dbo.ApoioImportacao", "LoteId", "dbo.Lote", "Id");
            AddForeignKey("dbo.ConsecutividadeImportacao", "LoteId", "dbo.Lote", "Id");
            AddForeignKey("dbo.IndicacaoImportacao", "LoteId", "dbo.Lote", "Id");
            DropColumn("dbo.ApoioImportacao", "ArquivoId");
            DropColumn("dbo.ConsecutividadeImportacao", "ArquivoId");
            DropColumn("dbo.IndicacaoImportacao", "ArquivoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IndicacaoImportacao", "ArquivoId", c => c.Int(nullable: false));
            AddColumn("dbo.ConsecutividadeImportacao", "ArquivoId", c => c.Int(nullable: false));
            AddColumn("dbo.ApoioImportacao", "ArquivoId", c => c.Int(nullable: false));
            DropForeignKey("dbo.IndicacaoImportacao", "LoteId", "dbo.Lote");
            DropForeignKey("dbo.ConsecutividadeImportacao", "LoteId", "dbo.Lote");
            DropForeignKey("dbo.ApoioImportacao", "LoteId", "dbo.Lote");
            DropIndex("dbo.IndicacaoImportacao", new[] { "LoteId" });
            DropIndex("dbo.ConsecutividadeImportacao", new[] { "LoteId" });
            DropIndex("dbo.ApoioImportacao", new[] { "LoteId" });
            DropColumn("dbo.IndicacaoImportacao", "LoteId");
            DropColumn("dbo.ConsecutividadeImportacao", "LoteId");
            DropColumn("dbo.ApoioImportacao", "LoteId");
            CreateIndex("dbo.IndicacaoImportacao", "ArquivoId");
            CreateIndex("dbo.ConsecutividadeImportacao", "ArquivoId");
            CreateIndex("dbo.ApoioImportacao", "ArquivoId");
            AddForeignKey("dbo.IndicacaoImportacao", "ArquivoId", "dbo.Arquivo", "Id");
            AddForeignKey("dbo.ConsecutividadeImportacao", "ArquivoId", "dbo.Arquivo", "Id");
            AddForeignKey("dbo.ApoioImportacao", "ArquivoId", "dbo.Arquivo", "Id");
        }
    }
}
