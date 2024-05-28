using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaLogArquivoMap : EntityTypeConfiguration<CampanhaLogArquivo>
    {
        public CampanhaLogArquivoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaLogArquivo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataInativacao).HasColumnName("DataInativacao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.CampanhaLogArquivoes)
                .HasForeignKey(d => d.ArquivoId);
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaLogArquivoes)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaLogArquivoes)
                .HasForeignKey(d => d.CampanhaPeriodoId);

        }
    }
}
