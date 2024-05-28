using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaGrupoItemMap : EntityTypeConfiguration<CampanhaGrupoItem>
    {
        public CampanhaGrupoItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaGrupoItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaGrupoItems)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.GrupoItem)
                .WithMany(t => t.CampanhaGrupoItem)
                .HasForeignKey(d => d.GrupoItemId);

        }
    }
}
