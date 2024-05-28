using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaSimuladorMap : EntityTypeConfiguration<CampanhaSimulador>
    {
        public CampanhaSimuladorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaSimulador");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.NumeroCampanha).HasColumnName("NumeroCampanha");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.LinkDownload).HasColumnName("LinkDownload");
            this.Property(t => t.AnoCampanha).HasColumnName("AnoCampanha");
        }
    }
}
