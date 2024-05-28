using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaAssociacaoGrupoItemImportacaoMap : EntityTypeConfiguration<CampanhaAssociacaoGrupoItemImportacao>
    {
        public CampanhaAssociacaoGrupoItemImportacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Perfil)
                .HasMaxLength(100);

            this.Property(t => t.Estrutura)
                .HasMaxLength(100);

            this.Property(t => t.GrupoItem)
                .HasMaxLength(100);

            this.Property(t => t.Erro)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("CampanhaAssociacaoGrupoItemImportacao");
            this.Property(t => t.Perfil).HasColumnName("Perfil");
            this.Property(t => t.Estrutura).HasColumnName("Estrutura");
            this.Property(t => t.GrupoItem).HasColumnName("GrupoItem");
            this.Property(t => t.ArquivoId).HasColumnName("ArquivoId");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GrupoItemId).HasColumnName("GrupoItemId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.EstruturaId).HasColumnName("EstruturaId");
            this.Property(t => t.Erro).HasColumnName("Erro");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasOptional(t => t.Estrutura1)
                .WithMany(t => t.CampanhaAssociacaoGrupoItemImportacaos)
                .HasForeignKey(d => d.EstruturaId);
            this.HasOptional(t => t.GrupoItem1)
                .WithMany(t => t.CampanhaAssociacaoGrupoItemImportacaos)
                .HasForeignKey(d => d.GrupoItemId);
            this.HasOptional(t => t.Perfil1)
                .WithMany(t => t.CampanhaAssociacaoGrupoItemImportacaos)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
