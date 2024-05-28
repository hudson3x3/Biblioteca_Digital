using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaGrupoItemPontoMap : EntityTypeConfiguration<CampanhaGrupoItemPonto>
    {
        public CampanhaGrupoItemPontoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaGrupoItemPontos");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.Multiplicador).HasColumnName("Multiplicador");
            this.Property(t => t.ValorPontos).HasColumnName("ValorPontos");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.DataInativacao).HasColumnName("DataInativacao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.Arquivo)
                .WithMany(t => t.CampanhaGrupoItemPontos)
                .HasForeignKey(d => d.ArquivoId);
            this.HasRequired(t => t.CampanhaEstrutura)
                .WithMany(t => t.CampanhaGrupoItemPontos)
                .HasForeignKey(d => d.CampanhaEstruturaId);
            this.HasRequired(t => t.CampanhaPerfil)
                .WithMany(t => t.CampanhaGrupoItemPontos)
                .HasForeignKey(d => d.CampanhaPerfilId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaGrupoItemPontos)
                .HasForeignKey(d => d.CampanhaPeriodoId);

        }
    }
}
