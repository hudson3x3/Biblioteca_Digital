using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaFaixaAtingimentoGrupoItemImportacaoMap : EntityTypeConfiguration<CampanhaFaixaAtingimentoGrupoItemImportacao>
    {
        public CampanhaFaixaAtingimentoGrupoItemImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Estrutura)
                .HasMaxLength(50);

            this.Property(t => t.Perfil)
                .HasMaxLength(50);

            this.Property(t => t.ItemCodigo)
                .HasMaxLength(50);

            this.Property(t => t.Erro)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("CampanhaFaixaAtingimentoGrupoItemImportacao");
            this.Property(t => t.Estrutura).HasColumnName("Estrutura");
            this.Property(t => t.Perfil).HasColumnName("Perfil");
            this.Property(t => t.ItemCodigo).HasColumnName("ItemCodigo");
            this.Property(t => t.ValorDe).HasColumnName("ValorDe");
            this.Property(t => t.ValorAte).HasColumnName("ValorAte");
            this.Property(t => t.Pontos).HasColumnName("Pontos");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.Erro).HasColumnName("Erro");
        }
    }
}
