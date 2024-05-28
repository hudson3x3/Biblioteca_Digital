using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPeriodoParticipanteEstruturaMap : EntityTypeConfiguration<CampanhaPeriodoParticipanteEstrutura>
    {
        public CampanhaPeriodoParticipanteEstruturaMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaPeriodoParticipanteEstrutura");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.Campanha)
                .WithMany(t => t.CampanhaPeriodoParticipanteEstruturas)
                .HasForeignKey(d => d.CampanhaId);
            this.HasOptional(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaPeriodoParticipanteEstruturas)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.Estrutura)
                .WithMany(t => t.CampanhaPeriodoParticipanteEstruturas)
                .HasForeignKey(d => d.EstruturaId);
            this.HasOptional(t => t.Participante)
                .WithMany(t => t.CampanhaPeriodoParticipanteEstruturas)
                .HasForeignKey(d => d.ParticipanteId);

        }
    }
}
