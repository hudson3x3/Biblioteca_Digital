using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstornoRequisicaoMap : EntityTypeConfiguration<EstornoRequisicao>
    {
        public EstornoRequisicaoMap()
        {
            ToTable("EstornoRequisicao");

            // Primary Key
            HasKey(t => t.Id);

            HasRequired(x => x.EstornoPedido).WithMany().HasForeignKey(x => x.EstornoPedidoId);
        }
    }
}
