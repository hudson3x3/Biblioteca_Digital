using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPassoMap : EntityTypeConfiguration<CampanhaPasso>
    {
        public CampanhaPassoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaPasso");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.PassoId).HasColumnName("PassoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAtualizacao).HasColumnName("DataAtualizacao");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaPassoes)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.Passo)
                .WithMany(t => t.CampanhaPassoes)
                .HasForeignKey(d => d.PassoId);

        }
    }
}
