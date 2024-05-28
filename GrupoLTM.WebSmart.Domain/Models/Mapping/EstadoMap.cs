using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstadoMap : EntityTypeConfiguration<Estado>
    {
        public EstadoMap()
        {
            // Primary Key
            this.HasKey(t => t.EstadoId);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Regiao)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Capital)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Estado");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Regiao).HasColumnName("Regiao");
            this.Property(t => t.Capital).HasColumnName("Capital");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
        }
    }
}
