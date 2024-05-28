using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(100);

            this.Property(t => t.Titulo)
                .HasMaxLength(50);

            this.Property(t => t.Link)
                .HasMaxLength(50);

            this.Property(t => t.Target)
                .HasMaxLength(50);

            this.Property(t => t.Icone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuPaiId).HasColumnName("MenuPaiId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Titulo).HasColumnName("Titulo");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.Target).HasColumnName("Target");
            this.Property(t => t.Icone).HasColumnName("Icone");
            this.Property(t => t.DataInicio).HasColumnName("DataInicio");
            this.Property(t => t.DataFim).HasColumnName("DataFim");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Ordem).HasColumnName("Ordem");

            // Relationships
            this.HasOptional(t => t.Menu2)
                .WithMany(t => t.Menu1)
                .HasForeignKey(d => d.MenuPaiId);

        }
    }
}
