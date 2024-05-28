using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMetaGrupoItemPerfilMap : EntityTypeConfiguration<CampanhaMetaGrupoItemPerfil>
    {
        public CampanhaMetaGrupoItemPerfilMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaMetaGrupoItemPerfil");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.ArquivoId);
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.CampanhaEstrutura)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.CampanhaEstruturaId);
            this.HasRequired(t => t.CampanhaPerfil)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.CampanhaPerfilId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasRequired(t => t.CampanhaMetaGrupoItemPerfil2)
                .WithOptional(t => t.CampanhaMetaGrupoItemPerfil1);
            this.HasRequired(t => t.GrupoItem)
                .WithMany(t => t.CampanhaMetaGrupoItemPerfils)
                .HasForeignKey(d => d.GrupoItemId);

        }
    }
}
