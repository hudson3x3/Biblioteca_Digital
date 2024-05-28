namespace GrupoLTM.WebSmart.Domain.Repository.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TamanhoParticipant : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Participante", "Nome", c => c.String(maxLength: 510));
            AlterColumn("dbo.Participante", "CNPJ", c => c.String(maxLength: 20));
            AlterColumn("dbo.Participante", "CPF", c => c.String(maxLength: 254));
            AlterColumn("dbo.Participante", "Sexo", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Participante", "Bairro", c => c.String(maxLength: 250));
            AlterColumn("dbo.Participante", "CEP", c => c.String(maxLength: 20));
            AlterColumn("dbo.Participante", "Cidade", c => c.String(maxLength: 250));
            AlterColumn("dbo.Participante", "Celular", c => c.String(maxLength: 20));
            AlterColumn("dbo.Participante", "Telefone", c => c.String(maxLength: 20));
            AlterColumn("dbo.Participante", "TelefoneComercial", c => c.String(maxLength: 20));
            AlterColumn("dbo.Participante", "Email", c => c.String(maxLength: 320));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Participante", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.Participante", "TelefoneComercial", c => c.String(maxLength: 8));
            AlterColumn("dbo.Participante", "Telefone", c => c.String(maxLength: 16));
            AlterColumn("dbo.Participante", "Celular", c => c.String(maxLength: 9));
            AlterColumn("dbo.Participante", "Cidade", c => c.String(maxLength: 100));
            AlterColumn("dbo.Participante", "CEP", c => c.String(maxLength: 8));
            AlterColumn("dbo.Participante", "Bairro", c => c.String(maxLength: 50));
            AlterColumn("dbo.Participante", "Sexo", c => c.String(maxLength: 1, fixedLength: true));
            AlterColumn("dbo.Participante", "CPF", c => c.String(maxLength: 11));
            AlterColumn("dbo.Participante", "CNPJ", c => c.String(maxLength: 14));
            AlterColumn("dbo.Participante", "Nome", c => c.String(maxLength: 255));
        }
    }
}
