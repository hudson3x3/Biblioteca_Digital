using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class MenuPerfilMap : EntityTypeConfiguration<MenuPerfil>
    {
        public MenuPerfilMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MenuPerfil");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.Menu)
                .WithMany(t => t.MenuPerfil)
                .HasForeignKey(d => d.MenuId);
            this.HasRequired(t => t.Perfil)
                .WithMany(t => t.MenuPerfils)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
