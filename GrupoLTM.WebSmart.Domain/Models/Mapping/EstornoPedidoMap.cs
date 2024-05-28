using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstornoPedidoMap : EntityTypeConfiguration<EstornoPedido>
    {
        public EstornoPedidoMap()
        {
            ToTable("EstornoPedido");

            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.CodigoProduto).IsRequired().HasMaxLength(50);
            Property(t => t.NomeProduto).IsRequired().HasMaxLength(200);

            HasRequired(x => x.EstornoLote).WithMany(x => x.EstornoPedidos).HasForeignKey(x => x.EstornoLoteId);
        }
    }
}
