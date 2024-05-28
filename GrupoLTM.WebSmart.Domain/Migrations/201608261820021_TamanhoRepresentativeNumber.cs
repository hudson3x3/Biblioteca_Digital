namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TamanhoRepresentativeNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentativeNumberDetail", c => c.String(maxLength: 5));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentiveAscNumberDetail", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentiveAscNumberDetail", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConsecutividadeDetail", "RepresentativeNumberDetail", c => c.String(maxLength: 4));
        }
    }
}
