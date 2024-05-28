using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaConteudoMap : EntityTypeConfiguration<CampanhaConteudo>
    {
        public CampanhaConteudoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Titulo)
                .HasMaxLength(30);

            this.Property(t => t.PreTexto)
                .HasMaxLength(255);

            this.Property(t => t.Arquivo)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CampanhaConteudo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.TipoconteudoId).HasColumnName("TipoconteudoId");
            this.Property(t => t.Ordem).HasColumnName("Ordem");
            this.Property(t => t.Titulo).HasColumnName("Titulo");
            this.Property(t => t.Texto).HasColumnName("Texto");
            this.Property(t => t.PreTexto).HasColumnName("PreTexto");
            this.Property(t => t.Arquivo).HasColumnName("Arquivo");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaConteudoes)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.TipoConteudo)
                .WithMany(t => t.CampanhaConteudoes)
                .HasForeignKey(d => d.TipoconteudoId);

        }
    }
}
