using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteEstruturaMap : EntityTypeConfiguration<ParticipanteEstrutura>
    {
        public ParticipanteEstruturaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ParticipanteEstrutura");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Estrutura)
                .WithMany(t => t.ParticipanteEstruturas)
                .HasForeignKey(d => d.EstruturaId);
            this.HasRequired(t => t.Participante)
                .WithMany(t => t.ParticipanteEstrutura)
                .HasForeignKey(d => d.ParticipanteId);
            this.HasOptional(t => t.Periodo)
                .WithMany(t => t.ParticipanteEstruturas)
                .HasForeignKey(d => d.PeriodoId);

        }
    }
}
