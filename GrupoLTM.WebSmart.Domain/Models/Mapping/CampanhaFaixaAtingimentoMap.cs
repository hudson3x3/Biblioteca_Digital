using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaFaixaAtingimentoMap : EntityTypeConfiguration<CampanhaFaixaAtingimento>
    {
        public CampanhaFaixaAtingimentoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaFaixaAtingimento");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.CampanhaPeriodoId).HasColumnName("CampanhaPeriodoId");
            this.Property(t => t.CampanhaEstruturaId).HasColumnName("CampanhaEstruturaId");
            this.Property(t => t.CampanhaPerfilId).HasColumnName("CampanhaPerfilId");
            this.Property(t => t.ValorDe).HasColumnName("ValorDe");
            this.Property(t => t.ValorAte).HasColumnName("ValorAte");
            this.Property(t => t.CalculaAtingimentoPercentual).HasColumnName("CalculaAtingimentoPercentual");
            this.Property(t => t.Pontos).HasColumnName("Pontos");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.CampanhaEstrutura)
                .WithMany(t => t.CampanhaFaixaAtingimentoes)
                .HasForeignKey(d => d.CampanhaEstruturaId);
            this.HasRequired(t => t.CampanhaPeriodo)
                .WithMany(t => t.CampanhaFaixaAtingimentoes)
                .HasForeignKey(d => d.CampanhaPeriodoId);
            this.HasRequired(t => t.CampanhaPerfil)
                .WithMany(t => t.CampanhaFaixaAtingimentoes)
                .HasForeignKey(d => d.CampanhaPerfilId);
            this.HasRequired(t => t.CampanhaPeriodo1)
                .WithMany(t => t.CampanhaFaixaAtingimentoes1)
                .HasForeignKey(d => d.CampanhaPeriodoId);

        }
    }
}
