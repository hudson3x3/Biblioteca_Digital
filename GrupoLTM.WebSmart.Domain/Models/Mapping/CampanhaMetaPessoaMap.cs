using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMetaPessoaMap : EntityTypeConfiguration<CampanhaMetaPessoa>
    {
        public CampanhaMetaPessoaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaMetaPessoa");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.ParticipanteId).HasColumnName("ParticipanteId");
            this.Property(t => t.Meta).HasColumnName("Meta");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaMetaPessoas)
                .HasForeignKey(d => d.CampanhaPeriodoId);

        }
    }
}
