using System.Data.Entity.ModelConfiguration;

namespace GrupoLTM.WebSmart.Domain.Models.Mapping
{
    public class CampanhaPerfilMap : EntityTypeConfiguration<CampanhaPerfil>
    {
        public CampanhaPerfilMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CampanhaPerfil");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampanhaId).HasColumnName("CampanhaId");
            this.Property(t => t.PerfilId).HasColumnName("PerfilId");
            this.Property(t => t.PerfilPontua).HasColumnName("PerfilPontua");
            this.Property(t => t.Participa).HasColumnName("Participa");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");

            // Relationships
            this.HasRequired(t => t.Campanha)
                .WithMany(t => t.CampanhaPerfil)
                .HasForeignKey(d => d.CampanhaId);
            this.HasRequired(t => t.Perfil)
                .WithMany(t => t.CampanhaPerfil)
                .HasForeignKey(d => d.PerfilId);

        }
    }
}
