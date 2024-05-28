namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndicacaoDetailExtratoAtivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndicacaoDetailExtrato", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IndicacaoDetailExtrato", "Ativo");
        }
    }
}
