namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PontuacaoOrigem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pontuacao", "IdOrigem", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pontuacao", "IdOrigem");
        }
    }
}
