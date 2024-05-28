using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class UsuarioAdmMenuMap : EntityTypeConfiguration<UsuarioAdmMenu>
    {
        public UsuarioAdmMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UsuarioAdmMenu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UsuarioAdmId).HasColumnName("UsuarioAdmId");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.Menu)
                .WithMany(t => t.UsuarioAdmMenu)
                .HasForeignKey(d => d.MenuId);
            this.HasRequired(t => t.UsuarioAdm)
                .WithMany(t => t.UsuarioAdmMenu)
                .HasForeignKey(d => d.UsuarioAdmId);

        }
    }
}
