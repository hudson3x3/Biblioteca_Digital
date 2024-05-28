using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class EstruturaMap : EntityTypeConfiguration<Estrutura>
    {
        public EstruturaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Tipo)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Estrutura");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PaiId).HasColumnName("PaiId");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.TipoEstruturaId).HasColumnName("TipoEstruturaId");

            // Relationships
            this.HasOptional(t => t.Estrutura2)
                .WithMany(t => t.Estrutura1)
                .HasForeignKey(d => d.PaiId);
            this.HasOptional(t => t.Periodo)
                .WithMany(t => t.Estruturas)
                .HasForeignKey(d => d.PeriodoId);
            this.HasOptional(t => t.TipoEstrutura)
                .WithMany(t => t.Estruturas)
                .HasForeignKey(d => d.TipoEstruturaId);

        }
    }
}
