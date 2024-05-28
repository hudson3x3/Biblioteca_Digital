using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaEstruturaMap : EntityTypeConfiguration<CampanhaEstrutura>
    {
        public CampanhaEstruturaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaEstrutura");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Participa).HasColumnName("Participa");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaEstrutura)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.Estrutura)
                .WithMany(t => t.CampanhaEstruturas)
                .HasForeignKey(d => d.EstruturaId);

        }
    }
}
