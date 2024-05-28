using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class ParticipanteCatalogoMap : EntityTypeConfiguration<ParticipanteCatalogo>
    {
        public ParticipanteCatalogoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ParticipanteId, t.CatalogoId });

            // Properties
            this.Property(t => t.ParticipanteId);
            this.Property(t => t.CatalogoId);
            this.Property(t => t.CodigoMktPlace);
            this.Property(t => t.Ativo);
            this.Property(t => t.DataInclusao);
            this.Property(t => t.DataAlteracao);

            // Table & Column Mappings
            this.ToTable("ParticipanteCatalogo");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.CatalogoId).HasColumnName("CatalogoId");
            this.Property(t => t.CodigoMktPlace).HasColumnName("CodigoMktPlace");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            this.HasRequired(t => t.Participante)
                .WithMany(t => t.ParticipanteCatalogos)
                .HasForeignKey(d => d.ParticipanteId);

            this.HasRequired(t => t.Catalogo)
                .WithMany(t => t.ParticipanteCatalogos)
                .HasForeignKey(d => d.CatalogoId);

        }
    }
}
