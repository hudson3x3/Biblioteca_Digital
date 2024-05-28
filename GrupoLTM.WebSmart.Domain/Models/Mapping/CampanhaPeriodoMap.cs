using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPeriodoMap : EntityTypeConfiguration<CampanhaPeriodo>
    {
        public CampanhaPeriodoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CampanhaPeriodo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.PeriodoDe).HasColumnName("PeriodoDe");
            this.Property(t => t.PeriodoAte).HasColumnName("PeriodoAte");
            this.Property(t => t.DataFechamento).HasColumnName("DataFechamento");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.Dataalteracao).HasColumnName("Dataalteracao");
            this.Property(t => t.Apurado).HasColumnName("Apurado");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaPeriodo)
                .HasForeignKey(d => d.CampanhaId);

        }
    }
}
