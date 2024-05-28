using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CatalogoMap : EntityTypeConfiguration<Catalogo>
    {
        public CatalogoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome).IsOptional();
            this.Property(t => t.Codigo);
            this.Property(t => t.Codigo);
            //this.Property(t => t.IdCampanha).IsOptional();
            //this.Property(t => t.IdOrigem).IsOptional();
            //this.Property(t => t.IdEmpresa).IsOptional();
            //this.Property(t => t.RepProfileType).IsOptional();
            //this.Property(t => t.MktPlaceSupplierId);

            // Table & Column Mappings
            this.ToTable("ParticipanteCatalogo");
            this.Property(t => t.Codigo).HasColumnName("MktPlaceCatalogoId") ;
            //this.Property(t => t.ConversionRate);
        }
    }
}
