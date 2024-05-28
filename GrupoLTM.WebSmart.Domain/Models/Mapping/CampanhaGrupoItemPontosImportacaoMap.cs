using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaGrupoItemPontosImportacaoMap : EntityTypeConfiguration<CampanhaGrupoItemPontosImportacao>
    {
        public CampanhaGrupoItemPontosImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DataInclusao, t.Id });

            // Properties
            this.Property(t => t.Estrutura)
                .HasMaxLength(255);

            this.Property(t => t.Perfil)
                .HasMaxLength(255);

            this.Property(t => t.ItemCodigo)
                .HasMaxLength(50);

            this.Property(t => t.Multiplicador)
                .HasMaxLength(50);

            this.Property(t => t.ValorPontos)
                .HasMaxLength(50);

            this.Property(t => t.Erro)
                .HasMaxLength(255);

            //this.Property(t => t.Id)
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("CampanhaGrupoItemPontosImportacao");
            this.Property(t => t.Estrutura).HasColumnName("Estrutura");
            this.Property(t => t.Perfil).HasColumnName("Perfil");
            this.Property(t => t.ItemCodigo).HasColumnName("ItemCodigo");
            this.Property(t => t.Multiplicador).HasColumnName("Multiplicador");
            this.Property(t => t.ValorPontos).HasColumnName("ValorPontos");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.CampanhaGrupoItemId).HasColumnName("CampanhaGrupoItemId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
