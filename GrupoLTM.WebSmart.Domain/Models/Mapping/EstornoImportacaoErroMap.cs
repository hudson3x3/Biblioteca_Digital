using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstornoImportacaoErroMap : EntityTypeConfiguration<EstornoImportacaoErro>
    {
        public EstornoImportacaoErroMap()
        {
            ToTable("EstornoImportacaoErro");

            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Linha).IsRequired();
            Property(t => t.DataInclusao).IsRequired();

            HasRequired(x => x.EstornoLote).WithMany().HasForeignKey(x => x.EstornoLoteId);
        }
    }
}
