using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteHierarquiaImportacaoMap : EntityTypeConfiguration<ParticipanteHierarquiaImportacao>
    {
        public ParticipanteHierarquiaImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LoginNivel1)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel2)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel3)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel4)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel5)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel6)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel7)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel8)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel9)
                .HasMaxLength(255);

            this.Property(t => t.LoginNivel10)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ParticipanteHierarquiaImportacao");
            this.Property(t => t.LoginNivel1).HasColumnName("LoginNivel1");
            this.Property(t => t.LoginNivel2).HasColumnName("LoginNivel2");
            this.Property(t => t.LoginNivel3).HasColumnName("LoginNivel3");
            this.Property(t => t.LoginNivel4).HasColumnName("LoginNivel4");
            this.Property(t => t.LoginNivel5).HasColumnName("LoginNivel5");
            this.Property(t => t.LoginNivel6).HasColumnName("LoginNivel6");
            this.Property(t => t.LoginNivel7).HasColumnName("LoginNivel7");
            this.Property(t => t.LoginNivel8).HasColumnName("LoginNivel8");
            this.Property(t => t.LoginNivel9).HasColumnName("LoginNivel9");
            this.Property(t => t.LoginNivel10).HasColumnName("LoginNivel10");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.ParticipanteId1).HasColumnName("ParticipanteId1");
            this.Property(t => t.ParticipanteId2).HasColumnName("ParticipanteId2");
            this.Property(t => t.ParticipanteId3).HasColumnName("ParticipanteId3");
            this.Property(t => t.ParticipanteId4).HasColumnName("ParticipanteId4");
            this.Property(t => t.ParticipanteId5).HasColumnName("ParticipanteId5");
            this.Property(t => t.ParticipanteId6).HasColumnName("ParticipanteId6");
            this.Property(t => t.ParticipanteId7).HasColumnName("ParticipanteId7");
            this.Property(t => t.ParticipanteId8).HasColumnName("ParticipanteId8");
            this.Property(t => t.ParticipanteId9).HasColumnName("ParticipanteId9");
            this.Property(t => t.ParticipanteId10).HasColumnName("ParticipanteId10");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Id).HasColumnName("Id");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.ParticipanteHierarquiaImportacaos)
                .HasForeignKey(d => d.ArquivoId);
            this.HasOptional(t => t.Periodo)
                .WithMany(t => t.ParticipanteHierarquiaImportacaos)
                .HasForeignKey(d => d.PeriodoId);

        }
    }
}
