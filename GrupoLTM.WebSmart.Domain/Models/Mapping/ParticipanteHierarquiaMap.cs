using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteHierarquiaMap : EntityTypeConfiguration<ParticipanteHierarquia>
    {
        public ParticipanteHierarquiaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ParticipanteHierarquia");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.ParticipanteIdPai).HasColumnName("ParticipanteIdPai");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.Participante)
                .WithMany(t => t.ParticipanteHierarquia)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Participante1)
                .WithMany(t => t.ParticipanteHierarquias1)
                .HasForeignKey(d => d.ParticipanteIdPai);
            this.HasRequired(t => t.Periodo)
                .WithMany(t => t.ParticipanteHierarquias)
                .HasForeignKey(d => d.PeriodoId);

        }
    }
}
