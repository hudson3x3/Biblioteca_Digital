using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstornoMap : EntityTypeConfiguration<Estorno>
    {
        public EstornoMap()
        {
            ToTable("Estorno");

            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Nome).IsRequired().HasMaxLength(150);
        }
    }
}
