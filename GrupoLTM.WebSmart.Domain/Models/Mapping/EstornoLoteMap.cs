using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstornoLoteMap : EntityTypeConfiguration<EstornoLote>
    {
        public EstornoLoteMap()
        {
            ToTable("EstornoLote");

            HasKey(x => x.Id);

            HasRequired(x => x.Estorno)
                .WithMany(x => x.Lotes)
                .HasForeignKey(y => y.EstornoId);
        }
    }
}
