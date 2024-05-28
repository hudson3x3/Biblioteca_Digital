using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPeriodoParticipantePerfilMap : EntityTypeConfiguration<CampanhaPeriodoParticipantePerfil>
    {
        public CampanhaPeriodoParticipantePerfilMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaPeriodoParticipantePerfil");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.Campanha)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils)
                .HasForeignKey(d => d.CampanhaId);
            this.HasOptional(t => t.Campanha1)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils1)
                .HasForeignKey(d => d.CampanhaId);
            this.HasOptional(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.CampanhaPeriodo1)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils1)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.Participante)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Perfil)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfil)
                .HasForeignKey(d => d.PerfilId);
            this.HasOptional(t => t.Participante1)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils1)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Perfil1)
                .WithMany(t => t.CampanhaPeriodoParticipantePerfils1)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
