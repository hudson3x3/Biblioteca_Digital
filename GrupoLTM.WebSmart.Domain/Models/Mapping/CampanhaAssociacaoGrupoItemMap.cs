using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaAssociacaoGrupoItemMap : EntityTypeConfiguration<CampanhaAssociacaoGrupoItem>
    {
        public CampanhaAssociacaoGrupoItemMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaAssociacaoGrupoItem");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.CampanhaEstrutura)
                .WithMany(t => t.CampanhaAssociacaoGrupoItems)
                .HasForeignKey(d => d.CampanhaEstruturaId);
            this.HasOptional(t => t.CampanhaPerfil)
                .WithMany(t => t.CampanhaAssociacaoGrupoItems)
                .HasForeignKey(d => d.CampanhaPerfilId);
            this.HasOptional(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaAssociacaoGrupoItems)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasOptional(t => t.GrupoItem)
                .WithMany(t => t.CampanhaAssociacaoGrupoItems)
                .HasForeignKey(d => d.GrupoItemId);

        }
    }
}
