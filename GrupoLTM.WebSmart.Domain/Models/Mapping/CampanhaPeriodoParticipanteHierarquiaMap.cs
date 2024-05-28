using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPeriodoParticipanteHierarquiaMap : EntityTypeConfiguration<CampanhaPeriodoParticipanteHierarquia>
    {
        public CampanhaPeriodoParticipanteHierarquiaMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaPeriodoParticipanteHierarquia");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.ParticipanteIdPai).HasColumnName("ParticipanteIdPai");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.Campanha)
                .WithMany(t => t.CampanhaPeriodoParticipanteHierarquias)
                .HasForeignKey(d => d.CampanhaId);
            this.HasOptional(t => t.Campanha1)
                .WithMany(t => t.CampanhaPeriodoParticipanteHierarquias1)
                .HasForeignKey(d => d.CampanhaId);
            this.HasOptional(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaPeriodoParticipanteHierarquias)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.Participante)
                .WithMany(t => t.CampanhaPeriodoParticipanteHierarquias)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Participante1)
                .WithMany(t => t.CampanhaPeriodoParticipanteHierarquias1)
                .HasForeignKey(d => d.ParticipanteIdPai);

        }
    }
}
