using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMetaParticipanteMap : EntityTypeConfiguration<CampanhaMetaParticipante>
    {
        public CampanhaMetaParticipanteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaMetaParticipante");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.CampanhaMetaParticipantes)
                .HasForeignKey(d => d.ArquivoId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaMetaParticipantes)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasRequired(t => t.Participante)
                .WithMany(t => t.CampanhaMetaParticipantes)
                .HasForeignKey(d => d.ParticipanteId);

        }
    }
}
