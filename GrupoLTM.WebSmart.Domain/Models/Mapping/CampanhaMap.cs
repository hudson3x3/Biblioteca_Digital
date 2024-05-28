using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaMap : EntityTypeConfiguration<Campanha>
    {
        public CampanhaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Descricao)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Campanha");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.DataInicio).HasColumnName("DataInicio");
            this.Property(t => t.DataFim).HasColumnName("DataFim");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.Dataalteracao).HasColumnName("Dataalteracao");
            this.Property(t => t.StatusCampanhaId).HasColumnName("StatusCampanhaId");
            this.Property(t => t.TipoCampanhaId).HasColumnName("TipoCampanhaId");
            this.Property(t => t.ResultadoCascata).HasColumnName("ResultadoCascata");
            this.Property(t => t.ExibirPeriodo).HasColumnName("ExibirPeriodo");
            this.Property(t => t.CalcularPelaHierarquia).HasColumnName("CalcularPelaHierarquia");
            this.Property(t => t.ExibirRankingIndividual).HasColumnName("ExibirRankingIndividual");

            // Relationships
            this.HasRequired(t => t.StatusCampanha)
                .WithMany(t => t.Campanhas)
                .HasForeignKey(d => d.StatusCampanhaId);
            this.HasOptional(t => t.TipoCampanha)
                .WithMany(t => t.Campanhas)
                .HasForeignKey(d => d.TipoCampanhaId);

        }
    }
}
