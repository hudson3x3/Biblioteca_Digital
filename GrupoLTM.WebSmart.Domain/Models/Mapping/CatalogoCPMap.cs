using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CatalogoCPMap : EntityTypeConfiguration<CatalogoCP>
    {
        public CatalogoCPMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CP);
            this.Property(t => t.ProfileId);
            this.Property(t => t.CatalogoId);
            this.Property(t => t.Ativo);
            this.Property(t => t.DataInicio);
            this.Property(t => t.DataFim);
            this.Property(t => t.Resgates);
            this.Property(t => t.DataAlteracao);

            // Table & Column Mappings
            this.ToTable("CatalogoCP");
            this.Property(t => t.CP).HasColumnName("CP");
            this.Property(t => t.ProfileId).HasColumnName("ProfileId");
            this.Property(t => t.CatalogoId).HasColumnName("CatalogoId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInicio).HasColumnName("DataInicio");
            this.Property(t => t.DataFim).HasColumnName("DataFim");
            this.Property(t => t.Resgates).HasColumnName("Resgates");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            this.HasRequired(t => t.Catalogo)
                .WithMany(t => t.CatalogoCPs)
                .HasForeignKey(d => d.CatalogoId);
        }
    }
}
