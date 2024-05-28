using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteImportacaoMap : EntityTypeConfiguration<ParticipanteImportacao>
    {
        public ParticipanteImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EstruturaNome)
                .HasMaxLength(255);

            this.Property(t => t.PerfilNome)
                .HasMaxLength(255);

            this.Property(t => t.Codigo)
                .HasMaxLength(255);

            this.Property(t => t.Login)
                .HasMaxLength(255);

            this.Property(t => t.Nome)
                .HasMaxLength(255);

            this.Property(t => t.CPF)
                .HasMaxLength(255);

            this.Property(t => t.RazaoSocial)
                .HasMaxLength(255);

            this.Property(t => t.CNPJ)
                .HasMaxLength(255);

            this.Property(t => t.DataNascimento)
                .HasMaxLength(255);

            this.Property(t => t.Email)
                .HasMaxLength(255);

            this.Property(t => t.DDDCel)
                .HasMaxLength(255);

            this.Property(t => t.Celular)
                .HasMaxLength(255);

            this.Property(t => t.Erro)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ParticipanteImportacao");
            this.Property(t => t.EstruturaNome).HasColumnName("EstruturaNome");
            this.Property(t => t.PerfilNome).HasColumnName("PerfilNome");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.CPF).HasColumnName("CPF");
            this.Property(t => t.RazaoSocial).HasColumnName("RazaoSocial");
            this.Property(t => t.CNPJ).HasColumnName("CNPJ");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.DDDCel).HasColumnName("DDDCel");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.ParticipanteImportacaos)
                .HasForeignKey(d => d.ArquivoId);
            this.HasOptional(t => t.Estrutura)
                .WithMany(t => t.ParticipanteImportacaos)
                .HasForeignKey(d => d.EstruturaId);
            this.HasOptional(t => t.Participante)
                .WithMany(t => t.ParticipanteImportacao)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Perfil)
                .WithMany(t => t.ParticipanteImportacaos)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
